# Import Accelerator (WIP)

## Overview
An unambition import-accelerator designed to facilitate the extraction, transformation, and loading (ETL) of data into a structured database. The import-accelerator implements a simple CDC using Object hashing.
The goal of this project is to provide a small framework that you can use to import data from a text or CSV file to Commercetools. 

This project is built with Dotnet, Mapster and entity framework core. 

## Features
- **Dynamic Entity Support:** Automatically detects and processes multiple entity types.
- **Dependency Injection:** Uses .NET DI to manage services efficiently. Almost everything is setup as open generics.
- **Parallel Processing:** Implements `Channels` for concurrent data processing.
- **Database Operations:** Supports table truncation and batch inserts.
- **Workflow Orchestration:** Utilizes a workflow manager to execute ETL tasks.

## Architecture
The pipeline consists of the following main components:

- **LandingDataConsumer:** Reads data from external sources and prepares it for database insertion.
- **LandingWorker:** Manages the execution of ETL tasks for a given entity type.
- **StagingDataConsumer:** Reads data from Landing table and prepares it for custom mapping to Staging entity types.
- **StagingWorker:** Manages the execution of ETL tasks for staging entity types.
- **FileConsumer:** Handles files processing, currently supports, TSV and CSV files.
- **DataBaseService:** Handles database interactions such as inserts and truncation.
- **Workflow Orchestrator:** Coordinates the execution of ETL tasks.

Architecture Diagram
![image](https://github.com/user-attachments/assets/76af1338-0aed-4761-a3b3-4a71e9f61919)

## Prerequisites
- .NET 9 or later
- Entity Framework Core
- A configured database (e.g., PostgreSQL, SQL Server)
- A valid `appsettings.json` configuration file

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/Nadben/Import-Accelerator.git
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Configure the database connection in `appsettings.json`:
   ```json
   {
        "AcceleratorConfiguration": {
          "SourceDirectory": "C:\\...\\Import-Accelerator\\Data",
          "PostgresConnectionString": ""
     }
   }
   ```

## Usage
To start the ETL process

- Drop the files in their respective Data folder (Import-Accelerator/Data/...)
- the source generators will read the headers and the first line of every file and will generate all the necessary models.
- Run database migrations using the power shell scripts (just add the name of the migration for the landing and staging DB).
- Write Custom mapping code for mapping between the landing models and staging models.
- run the application:
   
   ```sh
   dotnet run
   ```

The pipeline will:
1. Detect all entity types dynamically.
3. Register necessary services and consumers.
4. Execute workflows to process and load data.
