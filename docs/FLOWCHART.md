# Application Flowcharts — 01. Student Management System

These flowcharts match the implemented console application and can be rendered with any Mermaid viewer (VS Code, GitHub, [mermaid.live](https://mermaid.live)). Export screenshots into the Word report if required.

---

## 1. Main menu control flow

```mermaid
flowchart TD
    A([Start Program]) --> B[Load sample seed data]
    B --> C[Show Main Menu + live counters]
    C --> D[Read menu option]
    D --> E{Option?}

    E -->|1| F[Register Student]
    E -->|2| G[Create Course]
    E -->|3| H[Enroll Student]
    E -->|4| I[Drop Enrollment]
    E -->|5| J[List All Students]
    E -->|6| K[List All Courses]
    E -->|7| L[List All Enrollments]
    E -->|8| M[Search Student by Id]
    E -->|9| N[Search Course by Code]
    E -->|10| O[View Courses of a Student]
    E -->|11| P[View Course Roster]
    E -->|12| Q[Remove Student]
    E -->|13| R[Remove Course]
    E -->|14| S[Display Students by Program]
    E -->|0| T([Exit])
    E -->|other| U[Show invalid option error]

    F --> V[Pause / Enter]
    G --> V
    H --> V
    I --> V
    J --> V
    K --> V
    L --> V
    M --> V
    N --> V
    O --> V
    P --> V
    Q --> V
    R --> V
    S --> V
    U --> V
    V --> C
```

---

## 2. Enroll Student decision flow (core business rules)

```mermaid
flowchart TD
    A([Menu 3: Enroll Student]) --> B[Read Student Id and Course Code]
    B --> C{Student found?}
    C -->|No| R1[Reject: unknown student Id]
    C -->|Yes| D{Course found?}
    D -->|No| R2[Reject: unknown course code]
    D -->|Yes| E{Already enrolled?}
    E -->|Yes| R3[Reject: duplicate enrollment]
    E -->|No| F{Remaining seats > 0?}
    F -->|No| R4[Reject: course full]
    F -->|Yes| G{Student course count < 5?}
    G -->|No| R5[Reject: max 5 courses]
    G -->|Yes| H[Create Enrollment object]
    H --> I[Add to enrollments list]
    I --> S([Success message])

    R1 --> X([State unchanged])
    R2 --> X
    R3 --> X
    R4 --> X
    R5 --> X
```

---

## 3. Register Student validation flow

```mermaid
flowchart TD
    A([Menu 1: Register Student]) --> B[Choose Bachelor or Master]
    B --> C[Read Id, Name, Age, Degree Program]
    C --> D{Bachelor?}
    D -->|Yes| E[Read Study Year 1-4]
    D -->|No| F[Read Thesis Topic]
    E --> G{Id already exists?}
    F --> G
    G -->|Yes| R1[Reject: duplicate Id]
    G -->|No| H{Fields valid?\nId/Name/Program not empty\nAge 15-65}
    H -->|No| R2[Reject: invalid input]
    H -->|Yes| I{Bachelor year valid OR\nMaster thesis not empty?}
    I -->|No| R3[Reject: subclass validation]
    I -->|Yes| J[Create BachelorStudent / MasterStudent]
    J --> K[Add to students list]
    K --> S([Success])
    R1 --> X([State unchanged])
    R2 --> X
    R3 --> X
```

---

## How this satisfies the brief

- Shows class-operation behavior as a functional console workflow
- Makes success vs rejection paths explicit for testing and oral defense
- Complements the UML class diagram (structure) with runtime process flow (behavior)
