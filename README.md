# 01. Student Management System

C# / .NET 8 console app for OOP Case Study **01. Student Management System**.

**Authors:** Yigit Ali Engin, Nihat Yaman Celen

## Requirements

- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0

## Run

Windows:

```bat
run.cmd
```

Or:

```bat
dotnet run --project StudentManagementSystem.csproj
```

## Menu (matches the brief)

1. Register Student  
2. Create Course  
3. Enroll Student in Course  
4. Calculate Courses of a Student  
5. Display Students by Program  
6. Display Enrollment List  
7. List All Students (polymorphism demo)  
0. Exit  

## Project structure

```text
Program.cs
Interfaces/IIdentifiable.cs
Models/Student.cs
Models/BachelorStudent.cs
Models/MasterStudent.cs
Models/Course.cs
Models/Enrollment.cs
Services/UniversityManager.cs
```
