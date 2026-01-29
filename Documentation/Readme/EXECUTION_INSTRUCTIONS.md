# Execution Instructions

Below are the steps required to run the application using the docker‑compose file published on DockerHub as an OCI artifact. This process enables local deployment without installing any dependencies other than Docker.

---

## Prerequisites

To run the application, the following tools must be installed:

- Windows and macOS:  
Install Docker Desktop, which includes Docker Engine and Docker Compose.
Official installation page:
https://www.docker.com/products/docker-desktop/ 

- Linux:  
Install Docker and Docker Compose separately.
   - Docker Engine installation:
https://docs.docker.com/engine/install/  
   - Docker Compose installation:
https://docs.docker.com/compose/install/

Once installed, the environment is ready to run the application.

---

## Components

| Repository | Purpose | 
|-------|-------------|
| ivicenter2018/3dmanager-app | Application image |
| ivicenter2018/3dmanager-db  | MySQL image with embedded init.sql |
| ivicenter2018/3dmanager-app-compose | OCI artifact containing docker-compose.yml |

## Downloading the published docker‑compose
The docker-compose.yml file is distributed as an OCI artifact and can be downloaded using ORAS:

- **Install ORAS**

 ```curl -sSL https://oras.land/install.sh | sh```

- Pull the production docker-compose file

```oras pull registry-1.docker.io/ivicenter2018/3dmanager-app-compose:0.1```

This command retrieves the docker-compose.yml for the selected version.

> [!NOTE]  
> It is not necessary to manually download the image from DockerHub.
When running docker compose up, Docker will automatically pull the image specified in the docker-compose.yml, using the repository URL defined in the file.

## Before running the application

Create a .env file in the same directory as the docker-compose.yml, including the required environment variables (database configuration, JWT keys, etc.).

You must follow the next layout for the .env file:
```
# Database
DB_CONNECTION=<your BBDD connection string>

# JWT
JWT_KEY=<your JWT key>
JWT_ISSUER=<your JWT issuer>

# AWS
AWS_REGION=<your AWS bucket region>
AWS_BUCKET=<your AWS bucket name>
AWS_ACCESS_KEY=<your AWS access key>
AWS_SECRET_KEY=<your AWS secret key>

# AUTOMAPPER
AUTOMAPPER__LICENSE=<your License of Automapper>
# Certs
CERT_PASSWORD=3dmanagerPASS1234

```

## Start the application 

Exec the command : ```docker compose up -d```
Once the services are running, the application will be available at  ```http://localhost/```  on port 3000.

---

## 3DMANAGER Start

### Accessing the application (sample credentials)
The application includes a set of sample data accessible through guest mode, allowing users to explore the interface and view simulated content without registering.

Guest mode: direct access without credentials.

Also you can start with your own user creating a new one.

### Sample data included on guest version

Guest mode loads a demonstration group containing:

- Printers: predefined models illustrating the inventory structure.

- Filaments: various material types with basic properties.

- Users: a simulated list of group members.

- Printed parts: example items with automatically generated information, including data extracted from G-code files.

These sample datasets allow users to understand the application's behavior without creating real content.

