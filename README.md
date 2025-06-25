# 🌾 AgriEnergyConnect

**AgriEnergyConnect** is a web-based prototype designed to connect farmers and agricultural employees in a simple, secure, and efficient platform. 
It helps streamline the management of agricultural products and farmer profiles within a user-friendly web application built using ASP.NET Core MVC.

---

## Demo Video

Watch the full demonstration on YouTube:  
- [AgriEnergyConnect - YouTube Demo](https://www.youtube.com/watch?v=YOUR_VIDEO_LINK_HERE)

---

## 1. Setting Up the Development Environment

Follow these steps to set up and run the application:

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/)
  - With **ASP.NET and web development** workload installed
- A local SQL Server or **LocalDB** instance (default for Visual Studio)

### 🔧 Step-by-Step Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/AgriEnergyConnect.git
   cd AgriEnergyConnect
   ```

2. **Open the Project in Visual Studio**

- Open the .sln file (e.g. AgriEnergyConnect.sln).

3. **Restore NuGet Packages**

- Visual Studio should automatically restore all dependencies on load.
- Or manually from the Package Manager Console:

```bash 
dotnet restore
```

4. **Apply Database Migrations**

- Open Package Manager Console and run:

```bash 
Update-Database
```

This will apply Entity Framework Core migrations and create the database schema.

5. **Run the Application**

- Press F5 or click the green Start button in Visual Studio.
- The app will open in your browser at https://localhost:xxxx.

---

## 2. System Functionality & User Roles
AgriEnergyConnect supports two user roles with specific permissions:

### Farmer Role
**Features available:**

- View own products
- Add new products
- Edit existing products
- Delete products
- Farmers can only see and manage their own data.

### Employee Role
**Features available:**

- View all registered farmers
- Add new farmers (with password & location)
- Edit farmer details
- Delete farmers
- View and filter all products by:
  - Farmer
  - Product name
  - Category
  - Date range

### Authentication & Role Access

- Login and Registration use ASP.NET Identity.
- After logging in:
  - Navigation bar dynamically updates based on your role.
  - Unauthorized users are restricted from accessing protected pages.

---

## 3. UI & Styling Features

- Responsive layout using Bootstrap 5
- Clean, modern design with:
  - Role-specific welcome messages
  - Navigation bar with dynamic links
  - Confirmation cards for delete actions
- Visual cues for success and danger actions

---

## 4. Improvements Implemented

- Full UI redesign using modern Bootstrap layout
- Edit Product & Edit Farmer functionality added
- Delete Product added with confirmation card
- Dynamic navigation bar based on user role
- Smart Home screen logic based on login and role
- Improved layout, structure, and code readability

## 5. Folder Structure Overview

```bash
AgriEnergyConnect/
├── Area\Identity\Pages/
│   ├── Account/
│   │   ├── Login.cshtml
│   │   ├── Login.cshtml.cs
│   │   ├── Register.cshtml
│   │   ├── Register.cshtml.cs
│   │   └── RegisterViewModel.cs
│   └──
├── Bin/
├── Controllers/
│   ├── EmployeeController.cs
│   ├── FarmerController.cs
│   ├── HomeController.cs
│   └── ProductController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Migrations/
├── Models/
│   ├── ErrorViewModel.cs
│   ├── Farmer.cs
│   ├── FarmerViewModel.cs
│   └── Product.cs
├── Views/
│   ├── Employee/
│   ├── Farmer/
│   ├── Home/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
└── appsettings.json
```

## 6. Developer Info

- Developer: Asanda Dimba
- Institution: Varsity Colleg Westville
- Module: PROG7311 POE PART 3

## 7.License

This project is for educational purposes. You may reuse and adapt the code with attribution.
