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

Pick one:

- **Option A:** Docker Desktop (easiest on any PC)
- **Option B:** .NET SDK 8.0 or later

## How to run with Docker (recommended)

Install [Docker Desktop](https://www.docker.com/products/docker-desktop/), then from the project folder:

**Windows:**

```bat
run-docker.cmd
```

**macOS / Linux / any terminal:**

```bash
docker compose build
docker compose run --rm student-management
```

Or without compose:

```bash
docker build -t student-management-system .
docker run --rm -it student-management-system
```

Interactive menu needs `-it` / TTY. Stop with menu option `0` or `Ctrl+C`.

## How to run with .NET SDK

Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0), then from the project folder:

**Windows:**

```bat
run.cmd
```

**macOS / Linux:**

```bash
chmod +x run.sh
./run.sh
```

Or:

```bat
dotnet run --project StudentManagementSystem.csproj
```

### No .NET on the other PC? (Windows)

On a PC that **has** .NET 8 SDK, create a standalone EXE:

```bat
publish-win.cmd
```

Then copy `publish\win-x64\StudentManagementSystem.exe` to the other computer and double-click it. No SDK install needed there.

Sample fictional data is loaded at startup so the menu can be demonstrated immediately.

### Common fix if `run.cmd` fails on another PC

1. Install **.NET 8 SDK** (not only Visual Studio)
2. Open a new terminal and run `dotnet --version`
3. Run `run.cmd` again from the project folder (not from a random zip path with broken files)
4. If still stuck, use Docker (`run-docker.cmd`) or the standalone EXE from `publish-win.cmd`

## Project structure

```text
StudentManagementSystem/
├── Program.cs                      Console UI / menu
├── Dockerfile                      Container build
├── docker-compose.yml              Easy run on any PC
├── run-docker.cmd                  One-click Docker start (Windows)
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
