# Logistic Assistant
LogisticsAssistant is an ASP.NET MVC web application designed for fleet management and route scheduling. It allows users to manage a fleet of trucks, plan delivery routes, and visualize schedules through chart.

## Features
- **User Authentication**: Secure registration and login system for managing private fleet data.
- **Fleet Management**: Full CRUD operations for adding and managing transport vehicles.
- **Route Planning**: Create and track routes with automated duration calculations.
- **Gantt Chart Visualization**: Graphical representation of truck availability and route timelines for better resource management.
- **Service Testing**: Core business logic is covered by comprehensive unit tests to ensure reliability.

## Technologies
- **Backend**: ASP.NET Core MVC
- **Database**: SQL Server

## Installation and Setup
Clone the repository:
``` bash
git clone https://github.com/Rajfex/LogisticsAssistant.git
```
Open the solution file ```(.sln)``` in Visual Studio.

Update the connection string in appsettings.json.

Run migrations to set up the database:
```Update-Database```

Build and run the application.
