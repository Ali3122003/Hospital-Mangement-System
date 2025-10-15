#  Hospital Management System

A **web-based Hospital Management System** built using **.NET 8** and **SQL Server 2022**, designed to manage hospital operations efficiently.  
This system allows administrators and medical staff to handle patients, doctors, departments, appointments, rooms, and treatments — all through a clean, structured interface.

---

## 🚀 Features

- Manage **Patients**, **Doctors**, **Departments**, and **Rooms**  
- Record and track **Appointments** and **Treatments**  
- Fully structured **MVC architecture**  
- **Entity Framework Core** for database interaction  
- **Dockerized environment** for easy deployment  
- Pre-seeded database using `init.sql`

---

## 🧱 Technologies Used

- **.NET 8 (ASP.NET Core MVC)**
- **SQL Server 2022**
- **Entity Framework Core**
- **Docker & Docker Compose**

---

## 🗂️ Project Structure

```plaintext

HospitalManagementSystem/
├── HospitalManagementSystem/ # Main ASP.NET MVC web project
│ ├── Controllers/ # Application controllers
│ ├── Views/ # Razor views (UI)
│ ├── Models/ # Domain models
│ ├── ViewModels/ # View-specific models
│ ├── Middlewares/ # Custom middleware (error handling, etc.)
│ ├── appsettings.json # App configuration file
│ └── wwwroot/ # Static files (CSS, JS, images)
│
├── HospitalManagementSystem.Core/ # Core logic and specifications
├── HospitalManagementSystem.Repository/ # Data access layer (EF Core context)
├── HospitalManagementSystem.Services/ # Business services
│
├── Dockerfile # Docker build configuration
├── docker-compose.yml # Multi-container setup (web + db)
├── init.sql # Database initialization script
└── .env # Environment variables

```

---

## ⚙️ Setup & Run (Local Development)

### 1️⃣ Prerequisites
Make sure you have the following installed:
- [Docker](https://www.docker.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

---

### 2️⃣ Environment Variables
Create a file named `.env` in the project root:
```bash
SA_PASSWORD=
DB_NAME=
ASPNETCORE_ENVIRONMENT=
ACCEPT_EULA=
```


---

### 🐳 3️⃣ Run with Docker Compose

Build and run the application using the following command:

```bash
docker-compose up --build

```

---

### 🌐 4️⃣ Access the App

Once all containers are running successfully, open your browser and go to:

``` bash
http://localhost:5000
```

You should see the Hospital Management System homepage.

---


## 🏁 Conclusion

The **Hospital Management System** provides a robust, modular, and scalable foundation for managing hospital operations.  
Built with **.NET 8**, **Entity Framework Core**, and **Docker**, it ensures consistency between local and production environments.  


You can extend the system by adding authentication, cloud deployment (AWS, Azure), or CI/CD pipelines in the future.

---


## 🚀 Overview

The **Hospital Management System** is a web-based application built with **.NET 8** and **SQL Server 2022**, designed to streamline hospital operations and improve efficiency.  
It enables healthcare staff and administrators to manage patients, doctors, departments, rooms, appointments, and treatments — all from a centralized dashboard.

The project follows the **MVC architecture** pattern, uses **Entity Framework Core** for ORM and database interactions, and is fully containerized using **Docker and Docker Compose** for easy deployment across environments.
