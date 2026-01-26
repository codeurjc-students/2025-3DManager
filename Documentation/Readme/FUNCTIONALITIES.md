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

### Delete a group
A user is able to delete an existing group.
- Scope: Basic
- Users: Manager

### Join a group
A user is able to join an existing group by invitation of the owners of groups.
- Scope: Basic  
- Users: Base user  
- Implemented in v 0.1
  
### Leave a group
A user is able to leave a group. Their resources are no longer shown in listings but remain available in history and estimations.  
- Scope: Intermediate  
- Users: Base user  

### Transfer management
A user is able to transfer their manager role to another user in the group.
- Scope: Intermediate  
- Users: Manager  

---

## Dashboard

### Retrieve general data
A user is able to view a summary with aggregated printing metrics, including hours, 3DPrints, and material.
- Scope: Basic  
- Users: All  
  
### Retrieve group data for pop-up
A user is able to quickly view group statistics in pop-up windows. 
- Scope: Intermediate  
- Users: All  

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
- Scope: Intermediate  
- Users: All  

### Printer list
A user is able to view the printers associated with the groupand the basic data information of them
- Scope: Basic  
- Users: All  
- Implemented in v 0.1
  
---

## Details

### Filament details
A user is able to view complete information about a filament, including its consumption and associated 3D prints. 
- Scope: Intermediate  
- Users: All  

### User details
A user is able to view activity metrics of a user within the group.
- Scope: Intermediate  
- Users: All  

### Printer details
A user is able to view detailed information about a printer, including hours, 3D prints, and success rate.
- Scope: Intermediate  
- Users: All  

### 3dPrint details
A user is able to view printing metrics, including estimated vs. actual time, material used, and printer. 
- Scope: Intermediate  
- Users: All  

### 3D Print files
A user is able to view and download files related to a 3D print.
- Scope: Advanced
- Users: Base user, Manager

### 3dPrint comments
A user is able to write and view comments about a 3D print in the interaction section. 
- Scope: Intermediate  
- Users: Base user, Manager  

---

## Estimations and calculations

### Estimated real printing time based on history
A user is able to adjust predictions by comparing them with previously printed 3D prints. 
- Scope: Intermediate  
- Users: Base user, Manager  

### Estimated time variation per printer
A user is able to adjust printing times for each printer based on its historical data. 
- Scope: Intermediate  
- Users: Base user, Manager  

### Dashboard charts
A user is able to view visual representations of monthly usage, material consumption, and 3D prints. 
- Scope: Intermediate  
- Users: All  

### Printer detail chart – success rate
A user is able to view a visual indicator of the percentage of successful prints. 
- Scope: Intermediate  
- Users: All  

### Image processing
A user is able to associate and view photos of 3D prints, printers or users to complement the information. In v 0.1 only in the printers.
- Scope: Intermediate  
- Users: Base user, Manager  
- Implemented in v 0.1 (only on printers)
  
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
### Final List of developed

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
