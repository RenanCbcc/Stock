#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base 
# Fetch and install Node 14. Make sure to include the --yes parameter 
# to automatically accept prompts during install, or it'll fail.
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# Fetch and install Node 14. Make sure to include the --yes parameter 
# to automatically accept prompts during install, or it'll fail.
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs

WORKDIR /src
COPY ["Estoque.csproj", ""]
RUN dotnet restore "./Estoque.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Estoque.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Estoque.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Estoque.dll
