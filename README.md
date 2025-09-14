# 2025-3DManager
## Author and Supervisor

**Author:** Ismael Vicente Rodriguez  

**Email:** [i.vicenter.2018@alumnos.urjc.es](mailto:i.vicenter.2018@alumnos.urjc.es)

**Supervisor:** Michel Maes Bermejo  

**Email:** [michel.maes@urjc.es](mailto:michel.maes@urjc.es)

## Introduction
This web application is being developed as part of my final degree project for the Double Degree in Computer Engineering and Computer Science Engineering.

The developed web application aims to provide a comprehensive management platform for 3D printers and the materials used in the printing process.
Within this platform, users will share a common workspace as part of a group, where they can upload printed parts, comment on other members’ projects, and actively collaborate.
In addition, the system will deliver detailed insights on material usage and estimated printing times, ensuring efficient inventory tracking and production management within the group.
Ultimately, the goal is to centralize collaboration, monitoring, and optimization of 3D printing processes in a single, unified solution.


> At this stage, only the functional and technical objectives of the application have been defined. The development process has not yet started.

## Progress
**Current Phase** : 1
### Grant Diagram

<img width="1501" height="309" alt="image" src="https://github.com/user-attachments/assets/770a0e1f-3c7a-4d84-a8a1-996a3d43ef21" />

## Methodology
The project is developed in phases, structured as follows:

| Phase | Description | StartDate | EndDate | Deadline |
|-------|-------------|-----------|---------|----------|
| Phase 1 | Definition of functionalities and screens |September 01 |September 14 | September 15 |
| Phase 2 | Repository, testing, and CI | | | October 15 |
| Phase 3 | Version 0.1 - Basic functionality and Docker | | | December 15 |
| Phase 4 | Version 0.2 - Intermediate functionality | | | March 1 |
| Phase 5 | Version 1.0 - Advanced functionality | | | April 15 |
| Phase 6 | Report | | | May 15 |
| Phase 7 | Defense | | | June 15 |

### Phase Details

- **Phase 1 - Definition of functionalities and screens:** In this phase, the web functionalities and interaction design (screens, transitions, etc.) will be defined. Functionality will be differentiated according to user roles (guest, registered user, and administrator).
  
- **Phase 2 - Repository, testing, and CI:** The Git repository, client and server projects will be created, and minimal functionality to connect client, server, and database will be implemented. Minimal automated tests will be set up, and the CI system configured.

- **Phase 3 - Basic functionality and Docker:** Functionality will be extended to the basic features (with corresponding automated tests) and the application will be packaged in Docker. Continuous delivery will be added. Version 0.1 of the application will be released.

- **Phase 4 - Intermediate functionality:** Functionality will be extended to the intermediate features (with corresponding automated tests) and version 0.2 will be released. The application will also be deployed in this phase.

- **Phase 5 - Advanced functionality:** The application will be finalized and version 1.0 released.

- **Phase 6 - Report:** The first draft of the final report will be prepared.

- **Phase 7 - Defense:** The final project defense will take place.

## Objectives
### Functional Objectives
- Create users and allow registration with different roles and permissions.
- Implement CRUD operations for the entities: printers, spools, parts, and users.
- Visualize data extracted from 3D printing G-code files to provide technical insights into the printing process.
- Send notification emails to users, including alerts about comments on parts or depleted spools.
- Estimate printing times based on the history of parts and printers.
- Display user notes for printed parts.
- Develop a comprehensive system that enables the management and control of 3D printing processes.

### Technical Objectives: Architecture and Project Technologies

#### 1. General Structure
- **Architecture:** Monolithic with REST API implemented in ASP.NET Core (.NET 8), with logical layer separation
- **Frontend:** Single Page Application (SPA) implemented in React, communicating with the backend via HTTP calls to the REST API.  
- **Database:** MySQL, using stored procedures for all SQL logic.

#### 2. Detailed Technologies
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

#### 3. Data Flow
1. The user interacts with the React frontend.  
2. React makes HTTPS calls to the ASP.NET Core backend.  
3. The API calls the BLL to process business logic.  
4. The BLL calls the DAL, which executes stored procedures in MySQL via ADO.NET.  
5. Results are returned to the BLL, then to the API, and finally to the frontend to display data or reports.
   
## Functionalities 

### User
#### Create user
Allows registration in the application and creation of an account.  
- Scope: Basic  
- Users: All  

#### Log in as user
Access to the application with credentials, enabling access to group resources.  
- Scope: Basic  
- Users: Base user, Manager  

#### Log in as guest
Access in demo mode with mock data. Provides basic navigation to test the application.  
- Scope: Basic  
- Users: Guest  

---

### Group

#### Create group
Allows creation of a new group that will include users, printers, spools, and parts.  
- Scope: Basic  
- Users: Base user  

#### Join a group
Option for a user to join an existing group.  
- Scope: Basic  
- Users: Base user  

#### Leave a group
The user leaves a group. Their resources are no longer shown in listings but are kept in history and estimations.  
- Scope: Intermediate  
- Users: Base user  
 

#### Transfer management
The current manager can transfer their role to another user in the group.  
- Scope: Intermediate  
- Users: Manager  

---

### Dashboard

#### Retrieve general data
Summary with aggregated printing metrics (hours, parts, material).  
- Scope: Basic  
- Users: All  

#### Retrieve group data for pop-up
Quick view of group statistics in pop-up windows.  
- Scope: Intermediate  
- Users: All  

#### Upload part
Upload a GCODE file and generate a new part in the system with its metrics.  
- Scope: Basic  
- Users: Base user, Manager  

#### Create printer
Register a new printer in the group inventory.  
- Scope: Basic  
- Users: Manager  

#### Create spool
Register a new spool with its technical data.  
- Scope: Basic  
- Users: Manager  

#### Invite user to group
Send an invitation for another user to join the group.  
- Scope: Basic–Intermediate  
- Users: Manager  


---

### Listings

#### User list
Displays the users of the group.  
- Scope: Basic  
- Users: All  

#### Spool list
Displays the active spools in the group.  
- Scope: Basic  
- Users: All  

#### Parts list
Displays the parts associated with the group.  
- Scope: Basic  
- Users: All  

#### Messages list (app)
Displays notifications or internal app messages.  
- Scope: Intermediate  
- Users: All  

#### Printer list
Displays the printers associated with the group.  
- Scope: Basic  
- Users: All  

---

### Details

#### Spool details
Complete information about a spool, including consumption and printed parts with it.  
- Scope: Intermediate  
- Users: All  

#### User details
Activity metrics of a user within the group.  
- Scope: Intermediate  
- Users: All  

#### Printer details
Detailed information about the printer: hours, parts, success rate.  
- Scope: Intermediate  
- Users: All  

#### Part details
Printing metrics: estimated vs. actual time, material, printer used.  
- Scope: Intermediate  
- Users: All  

#### Part comments
Interaction section to write and view comments about a part.  
- Scope: Intermediate  
- Users: Base user, Manager  

---

### Estimations and calculations

#### Estimated real printing time based on history
Adjusts predictions by comparing with previously printed parts.  
- Scope: Intermediate  
- Users: Base user, Manager  

#### Estimated time variation per printer
Adjusts printing times for each printer based on its historical data.  
- Scope: Intermediate  
- Users: Base user, Manager  

#### Dashboard charts
Visual representations (monthly usage, material consumption, parts).  
- Scope: Intermediate  
- Users: All  

#### Printer detail chart – success rate
Visual indicator of the percentage of successful prints.  
- Scope: Intermediate  
- Users: All  

#### Image processing
Associate and display photos of printed parts to complement the information.  
- Scope: Intermediate  
- Users: Base user, Manager  

---

### Additional information

#### Email notification when spool is running low
Automatic notification to the manager when a spool is nearly depleted.  
- Scope: Advanced  
- Users: Manager  

#### Email notification when receiving a comment on a part
Automatic notification to the owner of a part when it receives a comment.  
- Scope: Advanced  
- Users: Base user, Manager  

#### Generate PDF when decommissioning a printer or spool
Document with the historical data of the decommissioned resource.  
- Scope: Advanced  
- Users: Manager  

## Analysis

### Screens and Navegation
An initial prototype of the application screens has been created using **Figma**.  
The prototype provides a visual overview of the different sections of the application and their interactions. **Its not the final version**

<img width="1283" height="925" alt="image" src="https://github.com/user-attachments/assets/95c288dd-be40-4b93-8ee4-b04c1037c7b2" />

---

This diagram shows the flow between screens, illustrating how users navigate through the application.

<img width="1086" height="779" alt="image" src="https://github.com/user-attachments/assets/5ffb25c6-3d79-4139-8f33-1f8d472dacb9" />

#### Interface Summary
- **Login:** Once the user logs in:  
  - If the user belongs to a group, they are directed to the group **dashboard**.  
  - If not, they are taken to screens to **create or join a group**.  

- **Dashboard:**  
  - Displays a list of printers and access to the rest of the inventory (spools, parts, users).  
  - Provides general visualization of key data and metrics.  
  - For **Managers**, additional options allow adding new inventory elements or users. Selecting the entity type will navigate to the corresponding creation screen.  
  - Files representing printed parts can be uploaded from the dashboard, available for all logged-in users. The upload screen shows all required fields for a part.  

- **Lists and Details:**  
  - From the inventory lists, users can access detailed information for each printer, spool, part, or user.  
  - Each entity has its own dedicated details screen.  

- **Navigation:**  
  - All screens include a **button that redirects to the dashboard**, ensuring easy access to the main application area at any time.

#### Screens
- **Log In**  
  - This screen represents the entry point for users and provides access to create an account or log in as a guest if they do not have one.  
  - Users with an account are directed to the dashboard, new users or those without a group are taken to the group screens. Guests access the dashboard directly.
    
<img width="982" height="729" alt="image" src="https://github.com/user-attachments/assets/78020479-26c7-44bc-b245-89de778bcdfe" />

- **User Create**  
  - This screen is used to create a new user account.  
  - Once the required information is submitted, the user is directed to the group section as a new member.

<img width="978" height="724" alt="image" src="https://github.com/user-attachments/assets/6b874dfd-5638-4a87-b38b-a9c956e3a4ef" />

- **Group**  
  - Allows viewing group invitations.  
  - Provides navigation to create a new group.

<img width="988" height="720" alt="image" src="https://github.com/user-attachments/assets/e7dabeb9-5e9c-4a1a-b04a-61b86dbda19e" />

- **Create Group**  
  - Allows users to create a group via a form.  
  - Once a group is created, the user is redirected to the dashboard to start managing inventory.

<img width="982" height="712" alt="image" src="https://github.com/user-attachments/assets/bf5d5e4b-a81c-420a-85e3-709ff7d21821" />

- **Dashboard – Base User**  
  - Displays general data and access to main sections.  
  - Allows access to upload parts, or view lists of printers, spools, users, and parts.

<img width="981" height="713" alt="image" src="https://github.com/user-attachments/assets/331df17b-e058-40cc-97e5-4ab83d157e3d" />

- **Dashboard – Manager**  
  - In addition to the base user functionalities, managers can access screens to **add inventory elements or users**.

<img width="978" height="721" alt="image" src="https://github.com/user-attachments/assets/03dbc132-d33d-42c7-bb63-525a4dedd788" />

- **Upload Part (GCODE Data)**  
  - Provides a form to create a new part.  
  - From this screen, users can go to the part’s detail page or return to the dashboard if they do not complete the action.

<img width="973" height="711" alt="image" src="https://github.com/user-attachments/assets/9de45e37-168d-4fb3-a685-79f17061c914" />

- **Upload inventory**  
  - Provides access to the uploads screens.

<img width="983" height="723" alt="image" src="https://github.com/user-attachments/assets/9b821481-d230-46b0-8cf0-51af50bb2c3e" />

- **Upload Printer**  
  - Provides a form to create a new printer.  
  - Users can either return to the dashboard or, after creating a printer, navigate to its detail screen.

<img width="971" height="715" alt="image" src="https://github.com/user-attachments/assets/152a86f9-da1e-4928-8f1e-609d2e4edebd" />

- **Upload Spool**  
  - Provides a form to create a new spool.  
  - Users can either return to the dashboard or navigate to the spool’s detail page after creation.

<img width="973" height="714" alt="image" src="https://github.com/user-attachments/assets/f560d3b6-75aa-4dfb-95fa-3eb3a28e6ce6" />

- **Invite Users**  
  - Allows inviting new users to the group.  
  - Users can only return to the dashboard from this screen.

<img width="973" height="715" alt="image" src="https://github.com/user-attachments/assets/76ef5ee9-04ae-4923-9277-9f675cf587e3" />

- **Spool List**  
  - Displays a list of spools in the group with basic information, inviting users to view spool details.  
  - Users can navigate to the spool details page or return to the dashboard.
  - 
<img width="975" height="720" alt="image" src="https://github.com/user-attachments/assets/614d4ff0-930f-4e38-abdd-86e0bfb0c043" />

- **User List**  
  - Displays a list of users in the group with basic information, inviting users to view user details.  
  - Users can navigate to the user details page or return to the dashboard.

<img width="988" height="721" alt="image" src="https://github.com/user-attachments/assets/77c6f031-18bf-49a7-8dd3-ffca46a6faac" />

- **Parts List**  
  - Displays a list of parts in the group with basic information, inviting users to view part details.  
  - Users can navigate to the part details page or return to the dashboard.

<img width="993" height="715" alt="image" src="https://github.com/user-attachments/assets/4aecde32-c9e0-4531-bd78-1003201127ed" />

- **Spool Details**  
  - Displays details about the spool and its usage in printing.  
  - Managers can change the status (e.g., deactivate) or edit some data.  
  - Users can navigate to the list of parts printed with this spool or return to the dashboard.

<img width="987" height="716" alt="image" src="https://github.com/user-attachments/assets/7493fc57-621b-4a52-b132-509c347689dc" />

- **Printer Details**  
  - Displays details about the printer and its usage.  
  - Managers can deactivate or edit printer information.  
  - Users can navigate to the list of parts printed with this printer or return to the dashboard.

<img width="973" height="715" alt="image" src="https://github.com/user-attachments/assets/4a5d30ee-bb1f-4c06-bf55-d86ddf3244b8" />

- **Part Details**  
  - Displays detailed information about a part and its printing process.  
  - Some values (like printing times) can be updated.  
  - Comments on the part can also be viewed.  
  - Users can return to the dashboard from this screen.

<img width="981" height="710" alt="image" src="https://github.com/user-attachments/assets/42449bb1-463d-4f87-9e7b-931b6d572208" />

- **User Details**  
  - Displays detailed information about the user and their printing activity.  
  - Users can view comments related to parts.  
  - Navigation to parts printed by the user or back to the dashboard is available.

<img width="973" height="713" alt="image" src="https://github.com/user-attachments/assets/578400d3-0274-4aa5-a8df-383a5fd48f5f" />

- **Log Out Pop-ups**  
  - Pop-ups that appear when the user clicks the log out button, including confirmation messages for irreversible actions.

<img width="972" height="449" alt="image" src="https://github.com/user-attachments/assets/89f6c2a5-2524-4259-ba72-8c9ddd8e114c" />

- **Messages**  
  - Pop-up displaying a list of short messages that inform the user about relevant events or notifications within the application.

<img width="481" height="601" alt="image" src="https://github.com/user-attachments/assets/cdf9b129-fdcb-412f-a5d6-0d64285fbe4f" />

- **Group Details Pop-up**  
  - Provides additional information about the group, complementing the dashboard overview.

<img width="582" height="595" alt="image" src="https://github.com/user-attachments/assets/454c238d-b9cb-48a1-946a-dced6b55f8c8" />

---
### Entities
This section introduces an initial diagram designed to visually support the previously described content. Its purpose is to clearly illustrate the relationships between the system’s main entities (users, groups, printers, spools, parts, and comments) and to provide a better understanding of the application’s logical architecture.
Below is an initial diagram that complements the content and entity definitions.

<img width="1332" height="780" alt="image" src="https://github.com/user-attachments/assets/81f0b121-16ff-480a-837a-0f49c52c6d75" />


#### 1.Users
Types of users:
- **Guest** → Access to the application in demo mode with mocked data. Can navigate but cannot interact with real data.  
- **Base User** → Standard user within a group. Can upload parts (GCODE), check printer and spool data, and comment on parts.  
- **Manager** → User with management permissions over the group. In addition to Base User actions, can create/remove printers, spools, and users within the group.  

#### 2.Group
- Main organizational unit.  
- A group contains: users, printers, spools, and parts.  
- Enables resource sharing and collaboration.  
- Each group has at least one **Manager** responsible for resource administration.  

#### 3.Printer
- Always associated with a group and linked to the parts printed on it.  
- Provides key metrics:  
  - Total printing hours.  
  - Printed parts (overall and per period).  
  - Variation between estimated and actual printing time (adjusted with history).  
- Acts as a central resource connecting production and material consumption.  

#### 4.Spool
- Represents a roll of filament material.  
- **Technical data:** filament type, color, diameter, ideal temperature, initial weight/length.  
- **Dynamic data:** remaining weight/length, printed parts using this spool, total consumption.  
- Directly linked to the parts printed with the material.  

#### 5.Part (GCODE Data)
- The actual production unit.  
- Generated from GCODE file analysis + user-provided data.  
- **Main information:**  
  - Estimated printing time (from slicer).  
  - Actual printing time (provided by user).  
  - Material consumption (linked spool).  
  - Printer used.  
  - Printing date.  
- Acts as the central node connecting printers, spools, and users.  

#### 6.Comment
- Provides the **social/collaborative layer** over printed parts.  
- Always associated with a part.  
- Facilitates feedback, recommendations, and discussions about printing results.
---
### Users 
#### 1.Guest User
- No registration required.  
- Accesses the application in demo mode with mocked (fictitious) data.  
- Can navigate through the main sections (dashboard, printers, spools, parts) to get an approximate user experience.  
- Cannot upload GCODE files, save data, or perform actions on real resources.  

#### 2.Base User
- Member of a group (organization).  
- Can navigate through all sections of their group: printers, spools, parts, and comments.  
- Upload GCODE files → automatic extraction of printing data.  
- View personal and group metrics (printed hours, material consumption, produced parts).  
- Check the status of printers and spools associated with the group.  
- Add comments on already printed parts.  

Does **not** have management permissions over the inventory or other users.  

#### 3.Manager User
- Administrator of the group.  
- Has the same permissions as a Base User (upload parts, check data, comment).  
- Additionally, has resource management functionalities:  
  - Add/remove printers.  
  - Add/remove spools.  
  - Add/remove users within the group.
---
### Images and graphics
#### Entity Images
Each main entity **spools, printers, parts, and users** will be represented by a image or icon.  
These visual representations serve for quick identification between users and they can instantly recognize the type of entity they are interacting with.  

#### Graphs and Charts
Visualizations will play a key role in making the system’s data more understandable and actionable. Initial implementations include:

- **General Dashboard:** A circular (pie) chart will display the total printing hours of each printer in the group. This allows managers and users to quickly assess the workload distribution among printers.  
- **Printer Details:** Each printer will feature a bar chart showing its success rate over time, providing immediate insight into reliability, and potential areas for maintenance or improvement.

---
#### Algrotims
The application will implement several algorithms to provide automatic calculations and estimations for 3D printing processes:

1. **Printer Success Rate**  
   Calculates the success rate of a printer based on the parts it prints and their status, automatically assessing performance.

2. **Estimated Printing Time**  
   Automatically estimates the actual time required to complete a part, based on historical printing data and trends.

3. **Spool Material Estimation**  
   Estimates the remaining material on a spool according to its usage in the application, allowing better inventory management.

4. **Part Cost Estimation**  
   Calculates the estimated cost of a printed part based on the material consumed from the spool used.

These algorithms aim to provide **automated insights**, help optimize resource usage, and assist users in planning and managing 3D printing tasks more efficiently.
