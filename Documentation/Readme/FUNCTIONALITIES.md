# Functionalities 

## User
### Create user
A user is able to register in the application and create an account. 
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
### Log in as user
A user is able to access the application with credentials, enabling access to group resources. 
- Scope: Basic  
- Users: Base user, Manager  
- Implemented in v 0.1
  
### Log in as guest
A guest user is able to access the application in demo mode with mock data, allowing basic navigation to test the application.  
- Scope: Basic  
- Users: Guest  
- Implemented in v 0.1
  
---

## Group

### Create group
A user is able to create a new group that includes users, printers, filaments, and 3D prints.
- Scope: Basic  
- Users: Base user  
- Implemented in v 0.1
  
### Edit a group
A user is able to edit the details of an existing group.
- Scope: Basic
- Users: Manager
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Delete a group
A user is able to delete an existing group.
- Scope: Basic
- Users: Manager
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Join a group
A user is able to join an existing group by invitation of the owners of groups.
- Scope: Basic  
- Users: Base user  
- Implemented in v 0.1
  
### Leave a group
A user is able to leave a group. Their resources are no longer shown in listings but remain available in history and estimations.  
- Scope: Intermediate  
- Users: Base user  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Transfer management
A user is able to transfer their manager role to another user in the group.
- Scope: Intermediate  
- Users: Manager  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

---

## Dashboard

### Retrieve general data
A user is able to view a summary with aggregated printing metrics, including hours, 3DPrints, and material.
- Scope: Basic  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0
  
### Retrieve group data for pop-up
A user is able to quickly view group statistics in pop-up windows. 
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Upload 3dPrint
A user is able to upload a GCODE file and generate a new 3D print in the system with its metrics. The relevant data of the file upload it can be seen by the user beofre extracction.   
- Scope: Basic  
- Users: Base user, Manager  
- Implemented in v 0.1
  
### Create printer
A manager user is able to register a new printer in the group inventory. The user can add a image to the printer entity 
- Scope: Basic  
- Users: Manager  
- Implemented in v 0.1
  
### Create Filament
A manager user is able to register a new filament along with its technical data. 
- Scope: Basic  
- Users: Manager  
- Implemented in v 0.1
  
### Invite user to group
A manager user is able to send an invitation to another user to join the group.
- Scope: Basic–Intermediate  
- Users: Manager  
- Implemented in v 0.1
  

---

## Listings

### User list
A user is able to view the users of the group and basic metric of them.
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
### Filaments list
A user is able to view the active filaments in the group and basic metric of them. 
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
### 3dPrints list
A user is able to view the 3D prints associated with the group and basic metric of them.  
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
### Notifications list (app)
A user is able to view app notifications.  
<<<<<<< HEAD
- Scope: Intermediate  
=======
- Scope: Advanced  
>>>>>>> release/0.2.0
- Users: All  

### Printer list
A user is able to view the printers associated with the groupand the basic data information of them
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
<<<<<<< HEAD
=======
### Pagination
Pagination has been implemented both on the server side for entities with potentially large data volumes—such as 3D prints—and on the client side for smaller entities like printers, where large quantities per group are not expected.
- Scope: Intermediate
- Users: All
- Implemented in: v0.2

### User search
A user is able to search for other users within the group to improve navigation and management efficiency.
- Scope: Basic
- Users: All
- Implemented in: v0.2

>>>>>>> release/0.2.0
---

## Details

### Filament details
A user is able to view complete information about a filament, including its consumption and associated 3D prints. 
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### User details
A user is able to view activity metrics of a user within the group.
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Printer details
A user is able to view detailed information about a printer, including hours, 3D prints, and success rate.
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### 3dPrint details
A user is able to view printing metrics, including estimated vs. actual time, material used, and printer. 
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### 3D Print files
A user is able to view and download files related to a 3D print.
- Scope: Advanced
- Users: Base user, Manager

### 3dPrint comments
A user is able to write and view comments about a 3D print in the interaction section. 
- Scope: Intermediate  
- Users: Base user, Manager  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

---

## Estimations and calculations

### Estimated real printing time based on history
A user is able to adjust predictions by comparing them with previously printed 3D prints. 
- Scope: Intermediate  
- Users: Base user, Manager  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Estimated time variation per printer
A user is able to adjust printing times for each printer based on its historical data. 
- Scope: Intermediate  
<<<<<<< HEAD
- Users: Base user, Manager  
=======
- Users: Base user, Manager 
- Implemented in v 0.2 
>>>>>>> release/0.2.0

### Dashboard charts
A user is able to view visual representations of monthly usage, material consumption, and 3D prints. 
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Printer detail chart – success rate
A user is able to view a visual indicator of the percentage of successful prints. 
- Scope: Intermediate  
- Users: All  
<<<<<<< HEAD
=======
- Implemented in v 0.2
>>>>>>> release/0.2.0

### Image processing
A user is able to associate and view photos of 3D prints, printers or users to complement the information. In v 0.1 only in the printers.
- Scope: Intermediate  
- Users: Base user, Manager  
- Implemented in v 0.1 (only on printers)
<<<<<<< HEAD
  
=======
- Implemented in v 0.2 All entities  
>>>>>>> release/0.2.0
---

## Additional information

### Email notification when filament is running low
A manager user is able to receive automatic notifications when a filament is nearly depleted.  
- Scope: Advanced  
- Users: Manager  

### Email notification when receiving a comment on a 3dPrint
A user is able to receive automatic notifications when a 3D print receives a comment.  
- Scope: Advanced  
- Users: Base user, Manager  

### Generate PDF when decommissioning a printer or filament
A user is able to view a document containing the historical data of a decommissioned resource. 
- Scope: Advanced  
- Users: Manager  

### Generate 3D Model of a 3dPrint
A user is able to view a 3D printed model of a 3dPrint uploaded in the group. 
- Scope: Advanced  
- Users: ALL

> **Note:** It will be developed according to the workload of the corresponding phase.

---

## Functionality v 0.1
Below is an overview of the main features showed in the video of this development version.

### Login
The application includes a login screen responsible for managing controlled access. Users can authenticate through the standard login form or enter as a guest, which provides a demo-like experience. A registration option is also available for new users.
Once authenticated, the application retrieves the user’s data and credentials, granting access exclusively to the group the user belongs to.

### Logout
The logout process ensures that the user exits the application securely. During this operation, all temporary data and credentials are removed from storage, preventing any sensitive information from remaining on the device.

### Guest Access
Guest mode allows users to explore the application without creating an account. Guests are placed into a demonstration group where they can view available content: printers, filament lists, group members, and printed parts.
As expected, guests do not have permission to add new content, including printed parts, which is a capability reserved for registered users.

### User Access (Group Owner)
This mode highlights the differences from guest access. A group owner has full creation privileges: they can add printers, filaments, and invite new users who are not yet part of any group.

### Viewing Group Content (Owner)
The owner can browse all elements associated with their group, including lists of printers, filaments, users, and printed parts. The dashboard also provides a summary of the available inventory.

### Content Creation (Owner)
Group owners can access creation forms and provide the required information to add new items to the group’s inventory. Unlike guest users, owners can also upload G-code files associated with printed parts.

### Content Creation (Group Member)
Group members can view the same information as the owner and are allowed to register new prints. However, they cannot expand the group’s inventory, meaning they are not permitted to create printers, filaments, or manage users.

---
<<<<<<< HEAD
### Final List of developed
=======

## Functionality v 0.2

### Edit Group  
Users with manager permissions can modify the basic details of an existing group.

### Delete Group  
Managers can remove a group entirely, ensuring a controlled and secure deletion process.

### Leave Group  
Base users can exit a group. Their resources are removed from active listings but remain available in history and estimations.

### Transfer Management  
A manager can delegate their role to another member of the group.

### General Data Summary  
All users can access aggregated printing metrics, including total hours, number of 3D prints, and material usage.

### Group Data Pop‑ups  
Quick‑access pop‑up windows provide immediate visibility into key group statistics.

### Filament Details  
Complete filament information, including consumption metrics and associated 3D prints.

### User Details  
Activity metrics for each user within the group.

### Printer Details  
Detailed printer statistics, including total hours, number of prints, and success rate.

### 3DPrint Details  
Metrics such as estimated vs. actual time, material usage, and the printer used.

### 3DPrint Comments  
Base users and managers can write and view comments in the interaction section of each print.

### Estimated Real Printing Time  
Predictions can be adjusted based on historical data from previously completed prints.

### Estimated Time Variation per Printer  
Users can get time estimates for each printer according to its historical performance.

### Dashboard Charts  
Visual representations of monthly usage, and number of 3D prints.

### Printer Success Rate Chart  
A visual indicator showing the percentage of successful prints for each printer when you pick a printer and a gcode file.

### Extended Image Support  
Users can associate and view photos for all entities (3D prints, filaments, users, and printers).
This expands the functionality introduced in Version 0.1, where images were supported only for printers.

### Reusable popup system
Users can view  information, warnings , view errors, or confirm actions in the UI through popups

### Pagination
Pagination has been implemented both on the server side for entities with potentially large data volumes—such as 3D prints—and on the client side for smaller entities like printers, where large quantities per group are not expected.

### User search
A user is able to search for other users within the group to improve navigation and management efficiency.
------
### Final List of developed 
>>>>>>> release/0.2.0

Version 0.1 delivers the initial set of core functionalities for user, group, and inventory management within the application. The full scope of this release includes:

- Implementation of Login and Logout, supported by JWT to ensure secure handling of user information.
- User registration through a dedicated sign‑up form.
- Group creation by registered users.
- Joining a group via invitation, with proper permission and membership handling.
- Access to the Dashboard, displaying key group information.
- Inventory listing, including:
   - Printers
   - Filaments
   - Users
   - Printed parts
- Printer creation (available to the group owner).
   - AWS S3 integration for image management (associated with printers for this version).
- Filament creation (group owner only).
- Inviting users to the group (group owner only).
- G-code file upload, including:
   - An algorithm to extract relevant data from the file to enhance the information shown to the user.
<<<<<<< HEAD
=======

Version 0.2 expands the application with advanced group management capabilities, detailed resource insights, and enhanced analytical tools. This release strengthens user autonomy and improves the depth of information available throughout the platform. The full scope of this release includes:

- Editing an existing group (manager only).
- Deleting a group (manager only).
- Leaving a group, removing the user’s resources from active listings while preserving historical data (base user).
- Transferring the manager role to another group member.
- Access to a general summary of aggregated printing metrics, including hours, number of 3D prints, and material usage.
- Filament details, including consumption metrics and associated 3D prints.
- User details, showing activity metrics within the group.
- Printer details, including total hours, number of prints, and success rate.
- 3DPrint details, including estimated vs. actual time, material usage, and printer information.
- 3DPrint comments, allowing users to write and view comments on each print (base user and manager).
- Estimated real printing time based on historical data.
- Estimated time variation per printer, adjusted according to its performance history.
- Dashboard charts showing monthly usage, material consumption, and number of 3D prints.
- Printer success rate chart, providing a visual indicator of print reliability.
- Image Processing
   - Extended image support for all entities (3D prints, filaments, users, and printers).This expands the functionality introduced in Version 0.1, where images were supported only for printers.
- General update and cleanup of warnings to make the code more reliable and aligned with good practices through static code analysis. This could not be applied in the same way as in other areas due to the initial development requirements, which were later adapted.
---
>>>>>>> release/0.2.0
