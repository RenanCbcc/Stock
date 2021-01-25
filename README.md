## Getting Started

After cloning the repository build the project to make sure everything is okay. Then, run the command to 
create the database and the tables. By default, it will create an SQServer database. 
To change that, navigate to appsettings.json

```
Update-Database
```

### Prerequisites

```
.Net Core 5.0
```

## Deployment at Heroku

Inside the stock project build the docker file using the following command.

```
docker build -t stock:1.1 .
```

After that, tag the heroku target image.

```
docker tag stock:1.1 registry.heroku.com/you_app_name/web
```

### Testing
To see if everything is okay before pushing the image to heroko, let's see if the image is working
as expected by running the falowing comand:

```
docker run -d -p 80:8080 stock:1.1
```
Then, type:

```
https://localhost:8080
```

### Pushing the docker image

To push push the docker image to heroku, type:

```
heroku container:push web -a you_app_name
```

Releasing the container on heroku.

```
heroku container:release web -a you_app_name
```

Change the launchSettings.json file. Replace the value of the ASPNETCORE_ENVIRONMENT property 
from Development to Production. That change will make the app use the Postgres database.

## Working demo

estoquapp.herokuapp.com/