# UML Class Diagram — 01. Student Management System

Matches the implemented C# code. View in VS Code Mermaid preview or https://mermaid.live

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
    }

    class BachelorStudent {
        -int _studyYear
        +int StudyYear
        +BachelorStudent(...)
        +GetAcademicLevel() string
        +GetStudentDetails() string
    }

    class MasterStudent {
        -string _thesisTopic
        +string ThesisTopic
        +MasterStudent(...)
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
        +Enrollment(...)
        +GetEnrollmentDetails() string
    }

    class UniversityManager {
        -List~Student~ _students
        -List~Course~ _courses
        -List~Enrollment~ _enrollments
        +RegisterBachelorStudent(...)
        +RegisterMasterStudent(...)
        +CreateCourse(...)
        +EnrollStudent(...)
        +GetStudentsByProgram(...)
        +CountEnrollmentsForStudent(...)
        +GetAllEnrollments(...)
    }

    IIdentifiable <|.. Student
    IIdentifiable <|.. Course
    Student <|-- BachelorStudent
    Student <|-- MasterStudent
    Enrollment --> Student : association
    Enrollment --> Course : association
    UniversityManager o-- Student
    UniversityManager o-- Course
    UniversityManager o-- Enrollment
```

## Relationships

| Relationship | Type | Meaning |
|--------------|------|---------|
| Bachelor/Master → Student | Inheritance | Specialized student types |
| Student/Course → IIdentifiable | Interface | Shared Id contract |
| Enrollment → Student/Course | Association | One enrollment links one student to one course |
| UniversityManager → lists | Ownership | In-memory registry for CRUD and rules |
