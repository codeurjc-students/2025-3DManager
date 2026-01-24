# Objectives

## Functional Objectives

- Create users and allow registration with different roles and permissions.
- Implement CRUD operations for the entities: printers, filaments, 3dPrints, and users.
- Visualize data extracted from 3D printing G-code files to provide technical insights into the printing process.
- Send notification emails to users, including alerts about comments on 3dPrints or depleted filaments.
- Estimate printing times based on the history of 3dPrints and printers.
- Display user notes for printed 3dPrints.
- Develop a comprehensive system that enables the management and control of 3D printing processes.

## Technical Objectives: Architecture and Project Technologies

### 1. General Structure

- **Architecture:** Monolithic with REST API implemented in ASP.NET Core (.NET 8), with logical layer separation
- **Frontend:** Single Page Application (SPA) implemented in React, communicating with the backend via HTTP calls to the REST API.  
- **Database:** MySQL, using stored procedures for all SQL logic.

### 2. Detailed Technologies

| Layer / Component | Technology / Tool | Notes |
|------------------|-----------------|-------|
| Backend (REST API) | ASP.NET Core Web API (.NET 8) | Controllers, BLL, and DAL(ADO.NET + stored procedures) organized in layers with clear separation of responsibilities |
| Frontend (SPA) | React | Communicates with backend via fetch or Axios, interactive and dynamic UI design |
| Database | MySQL | Stored procedures, initialization scripts, and sample data |
| Automated Testing | XUnit (backend), Selenium (UI) | Validation of main functionalities and API endpoints |
| API Documentation | Swagger/Postman | Interactive documentation with Swagger, Postman collection for interactive and exportable testing.|
| CI/CD | GitHub Actions | Build, test, and automatic deployment pipeline to Azure |
| Deployment | Azure App Service / Docker Container | Deploy backend and frontend, scalable and ready for load testing |
| Repository | GitHub |Version control |

### 3. Data Flow

1. The user interacts with the React frontend.  
2. React makes HTTPS calls to the ASP.NET Core backend.  
3. The API calls the BLL to process business logic.  
4. The BLL calls the DAL, which executes stored procedures in MySQL via ADO.NET.  
5. Results are returned to the BLL, then to the API, and finally to the frontend to display data or reports.
   