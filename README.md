# myESGIApi

## A dockerized, authenticated api service for the MyESGI app

The MyESGIApi is an Api service used by the MyESGI app for communicating with the database in a secure and confidential manner.


## Main functionalities
- JWT Authentication
- Secured and dockerised MSSQL instance 
- Clean file and code architecture 
- Up to date packages


## How to use 
### To use MyESGI API, simply use the following docker compose structure

```yaml
   services:
  my_esgi_api:
    image: adaouddev/my_esgi_api:latest
    container_name: my-esgi-api
    ports:
      - "8080:8080"
      - "8081:8081"
    env_file:
      - .env
    networks:
      - db-network
    depends_on:
      - mssql

  mssql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: mssql-container
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    networks:
      - db-network
    volumes:
      - mssql-data:/var/opt/mssql
    restart: always 
```
Afterwards, you need enter the correct credentials in the .env.example file and rename it to .env

Finally, run the following command: 
>  docker compose up -d
## License
This project falls under the MIT License and is free for commercial and non commercial use.
