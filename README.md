# COVID_19-WebApp-DotnetCore-MSSQL-JavaScript
This web application, developed during the COVID-19 pandemic, aims to keep the community informed about the impact and changes happening globally.

## Video Walkthrough
![](untitled.gif)

## Technical Specifications
This project implements an N-tier architecture, built upon the Model-View-Controller (MVC) architecture. However, instead of traditional Views, static HTML, CSS, and JavaScript files is used in the wwwroot directory. The architecture consists of the following layers:
### Presentation Layer
Static Files: HTML, CSS, and JavaScript files
### Application Layer
Model Layer: Stores the class structure and business logic  
Controller Layer: Serves the API requests that comes from the client side, apply some business logic and updates the Model.
### Data Access Layer (DAL)
Entity Framework: Utilizes DBContext to apply LINQ queries to the data received from the SQL database  
Data Exchange: Responsible for CRUD operations
### API Client Layer
Third-Party API Integration: Retrieves data from external API services  
Data Forwarding: Passes retrieved data to the DAL for processing and updating the databases

## Tech Stack
* HTML
* CSS
* JavaScript
* REST APIs
* C#
* .NETCORE
* Entity Framework
* LINQ
* MS-SQL
* Microsoft Azure  

## COVID-19 Web Application Features
The application offers the following features:

### Home Page
* Latest Tweets: Top Twitter handles related to COVID-19
* Global News: Updates from top publications worldwide
* Live Case Updates: Daily case numbers from your location and globally
### Cases Analytics
* Graphical Insights: New cases and deaths data from all countries and globally
* Country Comparison: Compare two countries' performance in controlling the pandemic
* Global Case List: Comprehensive list of case data from all countries
### Vaccine Analytics
* Vaccination Data: Similar format to Cases page, but for vaccination data
### News
* Latest News: Updates from The New York Times and top Twitter handles related to COVID-19
### Survey
* Work from Home Survey: Share your opinion and see live results and opinions from others


### Demo
https://youtu.be/SIGwO2x8p0E

### Home Page

<img width="800" alt="Screenshot 2021-11-24 at 10 40 28 PM" src="https://user-images.githubusercontent.com/48808040/143285245-043383ed-3b2c-4122-a020-72ff3489fc15.png">

### Home Page Continued

<img width="800" alt="Screenshot 2021-11-24 at 10 41 17 PM" src="https://user-images.githubusercontent.com/48808040/143285354-9224995d-c426-4019-95dc-cbb8c3d7b28c.png">

### Cases Analytics
<img width="800" alt="Screenshot 2021-11-24 at 10 42 07 PM" src="https://user-images.githubusercontent.com/48808040/143285445-f2df96d4-c348-41a7-b67a-97dca1c8b34f.png">

### Cases Analytics Continued
<img width="800" alt="Screenshot 2021-11-24 at 10 43 18 PM" src="https://user-images.githubusercontent.com/48808040/143285498-bd88a72b-8a29-4a0b-b87f-0ddd12ef1c26.png">

### Cases Analytics Continued
<img width="800" alt="Screenshot 2021-11-24 at 10 43 35 PM" src="https://user-images.githubusercontent.com/48808040/143285524-d904b7ea-77ba-4d0c-b1fa-e953e5e9d886.png">

### News and Twitter Updates

<img width="800" alt="Screenshot 2021-11-24 at 10 43 55 PM" src="https://user-images.githubusercontent.com/48808040/143285595-8b32824a-5ec9-4f7a-a0bb-acd46e5b5ab6.png">

### Survey

<img width="800" alt="Screenshot 2021-11-24 at 10 44 21 PM" src="https://user-images.githubusercontent.com/48808040/143285673-9a5da855-426f-4e98-8a4a-ac8699004d7e.png">

### About

<img width="800" alt="Screenshot 2021-11-24 at 10 44 56 PM" src="https://user-images.githubusercontent.com/48808040/143285730-dc3a287b-e0c4-4f94-b9b1-2972c4fc1df4.png">
