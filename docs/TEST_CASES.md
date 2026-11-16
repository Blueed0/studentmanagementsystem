# Functional Test Cases — 01. Student Management System

**Authors:** Yigit Ali Engin, Nihat Yaman Celen  
**Environment:** .NET 8 console application, in-memory `List<T>` storage  
**Execution date:** 16 November 2026  
**Runner:** `tools/run_functional_tests.py`  
**Evidence files:** `docs/TEST_RUN_SUMMARY.txt`, `docs/TEST_EVIDENCE.txt`  
**Seed data at startup:** S1001, S2001, S1002, courses CS101 (capacity 3), CS201, CS501, plus sample enrollments  
**Rule checked in every failed case:** system state must not change incorrectly

## Summary

| ID | Category | Result |
|----|----------|--------|
| TC01 | Normal – register Bachelor student | PASS |
| TC02 | Invalid input – age outside 15–65 | PASS |
| TC03 | Duplicate – same student Id | PASS |
| TC04 | Boundary – course capacity full | PASS |
| TC05 | Rejected – duplicate enrollment | PASS |
| TC06 | Normal – count courses / list by program | PASS |
| TC07 | Unknown Id – enroll missing student | PASS |
| TC08 | Boundary – age = 15 accepted | PASS |

**OVERALL: PASS** (verified by automated scripted console session)

---

### TC01 — Normal registration (Bachelor)

- **Preconditions:** Application running; Id `S3001` not used
- **Input:** Menu `1` → Bachelor → Id `S3001`, Name `Marie Curie`, Age `22`, Program `Physics`, Study Year `2`
- **Expected:** Student created; success message; appears in List Students
- **Actual:** Success message shown; `S3001` / Marie Curie present in evidence log
- **State check:** Student count increases by 1
- **Result:** **PASS**

### TC02 — Invalid age rejected

- **Preconditions:** Application running
- **Input:** Register student with Age `14`
- **Expected:** Operation rejected; age must be between 15 and 65; no invalid student stored until a valid age is entered
- **Actual:** Console message: `Value must be between 15 and 65.`
- **State check:** Invalid age does not create a student record by itself
- **Result:** **PASS**

### TC03 — Duplicate student Id rejected

- **Preconditions:** Seed student `S1001` already exists
- **Input:** Register another student with Id `S1001`
- **Expected:** Rejected as duplicate Id; original Ada Lovelace record unchanged
- **Actual:** `Student Id 'S1001' already exists.`
- **State check:** No second student with same Id
- **Result:** **PASS**

### TC04 — Course full (capacity boundary)

- **Preconditions:** Course `CS101` capacity = 3; seed already has one enrollment; two more seats filled by scripted students, then one extra attempt
- **Input:** Enroll additional student into `CS101` after capacity is reached
- **Expected:** Rejected because course is full
- **Actual:** `Course 'CS101' is full. No remaining seats.`
- **State check:** Enrollment count for CS101 does not exceed capacity
- **Result:** **PASS**

### TC05 — Duplicate enrollment rejected

- **Preconditions:** Seed enrollment `S1001` already enrolled in `CS201`
- **Input:** Menu `3` → Student `S1001`, Course `CS201`
- **Expected:** Rejected; no second enrollment row created
- **Actual:** `Student 'S1001' is already enrolled in course 'CS201'.`
- **State check:** Enrollment count for student unchanged by rejected attempt
- **Result:** **PASS**

### TC06 — Course count + students by program

- **Preconditions:** Seed data loaded (Computer Science has S1001 and S2001)
- **Input A:** Menu `10` → Student Id `S1001`
- **Expected A:** Shows enrolled courses and count against max 5
- **Actual A:** Student courses listed; enrolled course count shown in evidence
- **Input B:** Menu `14` → Program `Computer Science`
- **Expected B:** Lists matching Computer Science students
- **Actual B:** Computer Science filter output present in evidence log
- **Result:** **PASS**

### TC07 — Unknown student Id on enroll

- **Preconditions:** No student with Id `S9999`
- **Input:** Enroll `S9999` into `CS201`
- **Expected:** Rejected as unknown student; no enrollment created
- **Actual:** `Student 'S9999' was not found.`
- **State check:** Enrollment count unchanged
- **Result:** **PASS**

### TC08 — Boundary age 15 accepted

- **Preconditions:** Id `S3002` free
- **Input:** Register Bachelor with Age `15`
- **Expected:** Accepted (inclusive lower bound)
- **Actual:** Student `S3002` registered successfully
- **State check:** Student appears in later listings
- **Result:** **PASS**

---

## How to re-run

```bat
"C:\Program Files\dotnet\dotnet.exe" build StudentManagementSystem.csproj
python tools\run_functional_tests.py
```

Or manually with `run.cmd` and the menu steps above.
