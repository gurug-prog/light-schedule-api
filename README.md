# Light schedule API

## About

Simple webapi to view light schedules for blackout predictions. It can be useful for energy industiry companies or your local area experiments and so on

## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

To do all things you need to install the latest version of Docker with Docker Compose.

But you can also manually install all the dependencies (.NET 8, MS SQL Server) and configure them with secrets, connections strings, certificates and so on. Next you run it with you host environment.

### Installation

1. Clone the repo
   ```sh
   git clone git@github.com:gurug-prog/light-schedule-api.git
   ```
2. Move to project directory
   ```sh
   cd light-schedule-api
   ```
3. Run the `start.bat` script
   ```sh
   start.bat --build
   ```
   In future you can run it without `--build` argument. If you want to clear installed APIs dummy SSL-certificate just use `--clean` argument. 
4. Congratulations! Docker will download all dependencies and deploy the application.

## Usage

Before using API of the project you can fill the database with dummy values (addresses and light groups for correct work are required) and export your schedules.

Next you can start using this API using built-in Swagger UI by the link: https://localhost:5050/swagger/index.html

However, you can also do it in your own HTTP-client (curl, Postman, Insomnia, etc) sending requests to the APIs endpoints.

## Sql scripts

Examples of managing and manipulating with data of the relational model in SQL can be found [here](./sql-scripts).

