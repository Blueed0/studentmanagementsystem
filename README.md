# 01. Student Management System

C# / .NET 8 console application.

**Authors:** Yigit Ali Engin, Nihat Yaman Celen

## Run (any Windows PC)

1. Install **.NET 8 SDK** (not only Runtime): https://dotnet.microsoft.com/download/dotnet/8.0
2. Restart the PC
3. Open a **new** Command Prompt and check:

```bat
dotnet --version
```

If this fails with "dosya bulunamadığından yürütülemedi", Windows has a fake `dotnet` alias. Install the real SDK from the link above, then disable App execution aliases for `dotnet.exe` if needed:
Settings → Apps → Advanced app settings → App execution aliases → turn OFF `dotnet.exe`.

4. Clone/pull the repo, open the project folder, run:

```bat
run.cmd
```

or:

```bat
dotnet run --project StudentManagementSystem.csproj
```

If it still fails, open `.cursor\debug-a1d337.log` and share that file.

## Menu

1. Register Student  
2. Create Course  
3. Enroll Student in Course  
4. Calculate Courses of a Student  
5. Display Students by Program  
6. Display Enrollment List  
7. List All Students  
0. Exit  
