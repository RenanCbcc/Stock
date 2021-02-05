using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stock_Back_End.Filters;
using Stock_Back_End.Models;
using Stock_Back_End.Models.CategoryModels;
using Stock_Back_End.Models.ClientModels;
using Stock_Back_End.Models.OrderModels;
using Stock_Back_End.Models.PaymentModels;
using Stock_Back_End.Models.ProductModels;
using Stock_Back_End.Models.ReportModels;
using Stock_Back_End.Models.SupplierModels;
using System;
using System.Text;

namespace Stock_Back_End
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<StockContext>(options =>
            {
                if (env.IsDevelopment())
                {
                    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
                }
                if (env.IsProduction())
                {
                    options.UseNpgsql(configuration.GetConnectionString("PostgresqlConnection"));
                }
            });

            //Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<StockContext>()
                .AddDefaultTokenProviders();

            //Injection
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            //CORS Policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(configuration["Jwt:Audience"]);
                });
            });


            //Authentication and Autorization
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.FromHours(1)
                    };
                }
            );

            services.AddApiVersioning();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ErrorResponseFilter));
            });

            //I want to use my own state validation implemented in my exception filter.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            //APi documentation
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                //Fix enums conflicts.
                options.CustomSchemaIds(type => type.FullName);

                // definition of the security scheme used
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                //Defines what operations use the abome scheme - (all).
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }});

                options.OperationFilter<AuthResponseFilter>();

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Estoque api",
                    Description = "Documentação da api de estoque de produtos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Renan Rosa",
                        Email = "renannojosa@gmail.com"
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Stock API V2");
                options.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
