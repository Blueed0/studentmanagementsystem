# UML Class Diagram — 01. Student Management System

The diagram below matches the implemented C# solution. Render it in any Mermaid-compatible viewer (GitHub, VS Code Mermaid extension, mermaid.live), then export/screenshot for the PDF report if needed.

## Diagram

```mermaid
classDiagram
    direction TB

    class IIdentifiable {
        <<interface>>
        +string Id
    }

    class Student {
        <<abstract>>
        -string _id
        -string _fullName
        -int _age
        -string _degreeProgram
        +string Id
        +string FullName
        +int Age
        +string DegreeProgram
        #Student(id, fullName, age, degreeProgram)
        +GetAcademicLevel()* string
        +GetStudentDetails() string
        +ToString() string
    }

    class BachelorStudent {
        -int _studyYear
        +int StudyYear
        +BachelorStudent(id, fullName, age, degreeProgram, studyYear)
        +GetAcademicLevel() string
        +GetStudentDetails() string
    }

    class MasterStudent {
        -string _thesisTopic
        +string ThesisTopic
        +MasterStudent(id, fullName, age, degreeProgram, thesisTopic)
        +GetAcademicLevel() string
        +GetStudentDetails() string
    }

    class Course {
        -string _id
        -string _title
        -int _capacity
        +string Id
        +string Title
        +int Capacity
        +Course(id, title, capacity)
        +GetCourseDetails() string
        +ToString() string
    }

    class Enrollment {
        -string _enrollmentId
        -Student _student
        -Course _course
        -DateTime _enrollmentDate
        +string EnrollmentId
        +Student Student
        +Course Course
        +DateTime EnrollmentDate
        +Enrollment(enrollmentId, student, course, enrollmentDate)
        +GetEnrollmentDetails() string
        +ToString() string
    }

    class UniversityManager {
        -List~Student~ _students
        -List~Course~ _courses
        -List~Enrollment~ _enrollments
        +int MaxCoursesPerStudent$
        +RegisterBachelorStudent(...) BachelorStudent
        +RegisterMasterStudent(...) MasterStudent
        +FindStudentById(id) Student
        +GetAllStudents() List~Student~
        +GetStudentsByProgram(program) List~Student~
        +RemoveStudent(studentId) void
        +CreateCourse(id, title, capacity) Course
        +FindCourseById(id) Course
        +GetAllCourses() List~Course~
        +GetRemainingSeats(courseId) int
        +RemoveCourse(courseId) void
        +EnrollStudent(studentId, courseId) Enrollment
        +DropEnrollment(studentId, courseId) void
        +CountEnrollmentsForStudent(studentId) int
        +GetEnrollmentsByStudent(studentId) List~Enrollment~
        +GetEnrollmentsByCourse(courseId) List~Enrollment~
    }

    class Program {
        -UniversityManager _manager$
        +Main(args)$ void
    }

    IIdentifiable <|.. Student : realizes
    IIdentifiable <|.. Course : realizes
    Student <|-- BachelorStudent : inheritance
    Student <|-- MasterStudent : inheritance
    Enrollment --> Student : association
    Enrollment --> Course : association
    UniversityManager o-- Student : manages
    UniversityManager o-- Course : manages
    UniversityManager o-- Enrollment : manages
    Program --> UniversityManager : uses
```

## Relationships and multiplicities

| Relationship | Type | Multiplicity | Meaning |
|--------------|------|--------------|---------|
| `BachelorStudent` / `MasterStudent` → `Student` | Inheritance | 1 | Specialized student kinds reuse common fields |
| `Student` / `Course` → `IIdentifiable` | Realization | 1 | Shared read-only `Id` contract |
| `Enrollment` → `Student` | Association | many enrollments : 1 student | A student may have 0..5 enrollments |
| `Enrollment` → `Course` | Association | many enrollments : 1 course | A course may have 0..Capacity enrollments |
| `UniversityManager` → collections | Composition-like ownership | 1 manager owns many records | In-memory registry; UI never mutates lists directly |
| `Program` → `UniversityManager` | Dependency / use | 1 | Console UI delegates all business rules |

## Consistency with code

- Abstract `Student` + overrides in `BachelorStudent` / `MasterStudent` → polymorphism
- Private fields + validated properties → encapsulation
- `Enrollment` stores object references (not only IDs) → association / composition-style link object
- `UniversityManager` concentrates CRUD and rejection rules → clear class responsibility separation from UI
