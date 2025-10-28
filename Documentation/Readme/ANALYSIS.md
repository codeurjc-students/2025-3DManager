## Analysis

### Screens and Navegation
An initial prototype of the application screens has been created using **Figma**.  
The prototype provides a visual overview of the different sections of the application and their interactions. **Its not the final version**

![](Documentation/Screens/Screens.png)

---

This diagram shows the flow between screens, illustrating how users navigate through the application.

![](Documentation/Screens/ScreenNavigation.png)

#### Interface Summary
- **Login:** Once the user logs in:  
  - If the user belongs to a group, they are directed to the group **dashboard**.  
  - If not, they are taken to screens to **create or join a group**.  

- **Dashboard:**  
  - Displays a list of printers and access to the rest of the inventory (Filaments, 3dPrints, users).  
  - Provides general visualization of key data and metrics.  
  - For **Managers**, additional options allow adding new inventory elements or users. Selecting the entity type will navigate to the corresponding creation screen.  
 
- **Lists and Details:**  
  - From the inventory lists, users can access detailed information for each printer, filament, 3dPrint, or user.  
  - Each entity has its own dedicated details screen.  

- **Navigation:**  
  - All screens include a **button that redirects to the dashboard**, ensuring easy access to the main application area at any time.

#### Screens
- **Log In**  
  - This screen represents the entry point for users and provides access to create an account or log in as a guest if they do not have one.  
  - Users with an account are directed to the dashboard, new users or those without a group are taken to the group screens. Guests access the dashboard directly.
    
![](Documentation/Screens/LogIn.png)

- **User Create**  
  - This screen is used to create a new user account.  
  - Once the required information is submitted, the user is directed to the group section as a new member.

![](Documentation/Screens/CreateUser.png)

- **Group**  
  - Allows viewing group invitations.  
  - Provides navigation to create a new group.

![](Documentation/Screens/GroupScreen.png)

- **Create Group**  
  - Allows users to create a group via a form.  
  - Once a group is created, the user is redirected to the dashboard to start managing inventory.

![](Documentation/Screens/CreateGroup.png)

- **Dashboard – Base User**  
  - Displays general data and access to main sections.  
  - Allows access to upload 3dPrints, or view lists of printers, filaments, users, and 3dPrints.

![](Documentation/Screens/GeneralInfo.png)

- **Dashboard – Manager**  
  - In addition to the base user functionalities, managers can access screens to **add inventory elements or users**.

![](Documentation/Screens/GeneralInfoManager.png)

- **Upload 3dPrint**  
  - Provides a form to create a new 3dPrints.  
  - From this screen, users can go to the 3dPrints detail page or return to the dashboard if they do not complete the action.

![](Documentation/Screens/Upload3dPrint.png)

- **Upload inventory**  
  - Provides access to the uploads screens.

![](Documentation/Screens/UploadInventory.png)

- **Upload Printer**  
  - Provides a form to create a new printer.  
  - Users can either return to the dashboard or, after creating a printer, navigate to its detail screen.

![](Documentation/Screens/CreatePrinter.png)

- **Upload filaments**  
  - Provides a form to create a new filament.  
  - Users can either return to the dashboard or navigate to the filament’s detail page after creation.

![](Documentation/Screens/CreateFilament.png)

- **Invite Users**  
  - Allows inviting new users to the group.  
  - Users can only return to the dashboard from this screen.

![](Documentation/Screens/InviteUser.png)

- **Filaments List**  
  - Displays a list of filaments in the group with basic information, inviting users to view filament details.  
  - Users can navigate to the filaments details page or return to the dashboard.
   
![](Documentation/Screens/FilamentsList.png)

- **User List**  
  - Displays a list of users in the group with basic information, inviting users to view user details.  
  - Users can navigate to the user details page or return to the dashboard.

![](Documentation/Screens/UsersList.png)

- **3dPrints List**  
  - Displays a list of 3dPrints in the group with basic information, inviting users to view 3dPrint details.  
  - Users can navigate to the 3dPrint details page or return to the dashboard.

![](Documentation/Screens/3dPrintsList.png)

- **Filament Details**  
  - Displays details about the filament and its usage in printing.  
  - Managers can change the status (e.g., deactivate) or edit some data.  
  - Users can navigate to the list of 3dPrint printed with this filament or return to the dashboard.

![](Documentation/Screens/FilamentDetails.png)

- **Printer Details**  
  - Displays details about the printer and its usage.  
  - Managers can deactivate or edit printer information.  
  - Users can navigate to the list of 3dPrints printed with this printer or return to the dashboard.

![](Documentation/Screens/PrinterDetails.png)

- **3dPrint Details**  
  - Displays detailed information about a 3dPrint and its printing process.  
  - Some values (like printing times) can be updated.  
  - Comments on the 3dPrint can also be viewed.  
  - Users can return to the dashboard from this screen.

![](Documentation/Screens/3DPrintDetails.png)

- **User Details**  
  - Displays detailed information about the user and their printing activity.  
  - Users can view comments related to 3dPrints.  
  - Navigation to 3dPrints printed by the user or back to the dashboard is available.

![](Documentation/Screens/UserDetails.png)

- **Log Out Pop-ups**  
  - Pop-ups that appear when the user clicks the log out button, including confirmation messages for irreversible actions.

![](Documentation/Screens/LogOut.png)

- **Notifications**  
  - Pop-up displaying a list of short notifications that inform the user about relevant events or notifications within the application.

![](Documentation/Screens/Notifications.png)

- **Group Details Pop-up**  
  - Provides additional information about the group, complementing the dashboard overview.

![](Documentation/Screens/GroupDetails.png)

---
### Entities
This section introduces an initial diagram designed to visually support the previously described content. Its purpose is to clearly illustrate the relationships between the system’s main entities (users, groups, printers, filaments, 3dPrints, and comments) and to provide a better understanding of the application’s logical architecture.
Below is an initial diagram that complements the content and entity definitions.

![](Documentation/Diagrams/ERDiagram.png)

#### 1.Users
Types of users:
- **Guest** : Access to the application in demo mode with mocked data. Can navigate but cannot interact with real data.  
- **Base User** : Standard user within a group. Can upload 3dPrints (GCODE & STL), check printer and filaments data, and comment on 3dPrints.  
- **Manager** : User with management permissions over the group. In addition to Base User actions, can create/remove printers, filaments, and users within the group.  

#### 2.Group
- Main organizational unit.  
- A group contains: users, printers, filaments, and 3dPrints.  
- Enables resource sharing and collaboration.  
- Each group has at least one **Manager** responsible for resource administration.  

#### 3.Printer
- Always associated with a group and linked to the 3dPrints printed on it.  
- Provides key metrics:  
  - Total printing hours.  
  - Printed 3dPrints (overall and per period).  
  - Variation between estimated and actual printing time (adjusted with history).  
- Acts as a central resource connecting production and material consumption.  

#### 4.Filament
- Represents a roll of filament material.  
- **Technical data:** filament type, color, diameter, ideal temperature, initial weight/length.  
- **Dynamic data:** remaining weight/length, printed 3dPrints using this filament, total consumption.  
- Directly linked to the 3dPrints printed with the material.  

#### 5.3dPrint (GCODE Data and STL)
- The actual production unit.  
- Generated from GCODE file analysis + user-provided data.  
- **Main information:**  
  - Estimated printing time (from slicer).  
  - Actual printing time (provided by user).  
  - Material consumption (linked filament).  
  - Printer used.  
  - Printing date.  
- Acts as the central node connecting printers, filaments, and users.  

#### 6.Comment
- Provides the **social/collaborative layer** over printed 3dPrints.  
- Always associated with a 3dPrint.  
- Facilitates feedback, recommendations, and discussions about printing results.
---
### Users 
#### 1.Guest User
- No registration required.  
- Accesses the application in demo mode with mocked (fictitious) data.  
- Can navigate through the main sections (dashboard, printers, filaments, 3dPrints) to get an approximate user experience.  
- Cannot upload GCODE files, save data, or perform actions on real resources.  

#### 2.Base User
- Member of a group (organization).  
- Can navigate through all sections of their group: printers, filaments, 3dPrints, and comments.  
- Upload GCODE files : automatic extraction of printing data.  
- View personal and group metrics (printed hours, material consumption, produced 3dPrints).  
- Check the status of printers and filaments associated with the group.  
- Add comments on already printed 3dPrints.  

Does **not** have management permissions over the inventory or other users.  

#### 3.Manager User
- Administrator of the group. **Is the owner of the group** 
- Has the same permissions as a Base User (upload 3dPrints, check data, comment).  
- Additionally, has resource management functionalities:  
  - Add/remove printers.  
  - Add/remove filaments.  
  - Add/remove users within the group.
---
### Images and graphics
#### Entity Images
Each main entity **filaments, printers, 3dPrints, and users** will be represented by a image or icon.  
These visual representations serve for quick identification between users and they can instantly recognize the type of entity they are interacting with.  

#### Graphs and Charts
Visualizations will play a key role in making the system’s data more understandable and actionable. Initial implementations include:

- **General Dashboard:** A circular (pie) chart will display the total printing hours of each printer in the group. This allows managers and users to quickly assess the workload distribution among printers.  
- **Printer Details:** Each printer will feature a bar chart showing its success rate over time, providing immediate insight into reliability, and potential areas for maintenance or improvement.

---
#### Algorithms
The application will implement several algorithms to provide automatic calculations and estimations for 3D printing processes:

1. **Printer Success Rate**  
   Calculates the success rate of a printer based on the 3dPrints it prints and their status, automatically assessing performance.

2. **Estimated Printing Time**  
   Automatically estimates the actual time required to complete a 3dPrint, based on historical printing data and trends.

3. **Filament Material Estimation**  
   Estimates the remaining material on a filament according to its usage in the application, allowing better inventory management.

4. **3dPrint Cost Estimation**  
   Calculates the estimated cost of a printed 3dPrint based on the material consumed from the filament used.

These algorithms aim to provide **automated insights**, help optimize resource usage, and assist users in planning and managing 3D printing tasks more efficiently.
