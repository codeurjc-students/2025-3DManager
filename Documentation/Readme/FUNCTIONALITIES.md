# Functionalities 

## User
### Create user
A user is able to register in the application and create an account. 
- Scope: Basic  
- Users: All  

### Log in as user
A user is able to access the application with credentials, enabling access to group resources. 
- Scope: Basic  
- Users: Base user, Manager  

### Log in as guest
A guest user is able to access the application in demo mode with mock data, allowing basic navigation to test the application.  
- Scope: Basic  
- Users: Guest  

---

## Group

### Create group
A user is able to create a new group that includes users, printers, filaments, and 3D prints.
- Scope: Basic  
- Users: Base user  

### Edit a group
A user is able to edit the details of an existing group.
- Scope: Basic
- Users: Manager

### Delete a group
A user is able to delete an existing group.
- Scope: Basic
- Users: Manager

### Join a group
A user is able to join an existing group.
- Scope: Basic  
- Users: Base user  

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
A user is able to upload a GCODE file and generate a new 3D print in the system with its metrics.  
- Scope: Basic  
- Users: Base user, Manager  

### Create printer
A manager user is able to register a new printer in the group inventory. 
- Scope: Basic  
- Users: Manager  

### Create Filament
A manager user is able to register a new filament along with its technical data. 
- Scope: Basic  
- Users: Manager  

### Invite user to group
A manager user is able to send an invitation to another user to join the group.
- Scope: Basic–Intermediate  
- Users: Manager  


---

## Listings

### User list
A user is able to view the users of the group. 
- Scope: Basic  
- Users: All  

### Filaments list
A user is able to view the active filaments in the group. 
- Scope: Basic  
- Users: All  

### 3dPrints list
A user is able to view the 3D prints associated with the group.  
- Scope: Basic  
- Users: All  

### Notifications list (app)
A user is able to view app notifications.  
- Scope: Intermediate  
- Users: All  

### Printer list
A user is able to view the printers associated with the group.  
- Scope: Basic  
- Users: All  

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
A user is able to associate and view photos of 3D prints to complement the information. 
- Scope: Intermediate  
- Users: Base user, Manager  

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
