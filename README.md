# 01. Student Management System

C# / .NET 8 console application for the OOP midterm case study **01. Student Management System**.

## What it does

- Register **Bachelor** and **Master** students with validated fields
- Create courses with unique codes and positive capacity
- Enroll / drop students with business-rule checks
- List and search students, courses, and enrollments
- Display students by degree program
- View a student's courses and a course roster
- Safely remove students/courses only when no active enrollments exist

## Requirements

- .NET SDK 8.0 or later
- Windows / macOS / Linux terminal

## How to run

From the project folder:

```bat
run.cmd
```

Or:

```bat
"C:\Program Files\dotnet\dotnet.exe" run --project StudentManagementSystem.csproj
```

Sample fictional data is loaded at startup so the menu can be demonstrated immediately.

## Project structure

```text
StudentManagementSystem/
├── Program.cs                      Console UI / menu
├── Interfaces/IIdentifiable.cs     Shared Id contract
├── Models/
│   ├── Student.cs                  Abstract base student
│   ├── BachelorStudent.cs
│   ├── MasterStudent.cs
│   ├── Course.cs
│   └── Enrollment.cs               Student–Course association
├── Services/UniversityManager.cs   Business rules / registry
└── docs/                           Report, UML, tests, statements
```

## Documentation package

| File | Purpose |
|------|---------|
| `docs/REPORT.md` | Full assessment report (Tasks 1–6) |
| `docs/UML_CLASS_DIAGRAM.md` | UML class diagram (Mermaid + notes) |
| `docs/FLOWCHART.md` | Main menu + enrollment/register flowcharts |
| `docs/TEST_CASES.md` | Functional test evidence (≥ 6 cases) |
| `docs/TEST_RUN_SUMMARY.txt` | Automated test PASS/FAIL summary |
| `docs/TEST_EVIDENCE.txt` | Raw console output from scripted tests |
| `docs/AI_USE_STATEMENT.md` | AI-use disclosure |
| `docs/MEMBER_CONTRIBUTIONS.md` | Author contribution statement |
| `submission/*.docx` | Word exports ready for professor submission |

## Business rules (summary)

1. Student Id and Course code must be unique.
2. Age must be between **15 and 65** (inclusive).
3. Course capacity must be **greater than 0**.
4. A student may enroll in a course **at most once**.
5. A student may enroll in at most **5** courses.
6. Enrollment is rejected when the course has **no remaining seats**.
7. Remove student/course is rejected while related enrollments still exist.

## Submission notes

- Platform: C# / .NET Console Application
- Data: fictional only
- Database / GUI / API: not required and not used
