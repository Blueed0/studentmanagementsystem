# Functional Test Cases — 01. Student Management System

**Authors:** Yigit Ali Engin, Nihat Yaman Celen  
**Date:** 18 November 2026  
**Rule for failed cases:** system state must not change incorrectly

## Summary

| ID | Category | Result |
|----|----------|--------|
| TC01 | Normal – register Bachelor student | PASS |
| TC02 | Invalid input – age outside 15–65 | PASS |
| TC03 | Duplicate student Id | PASS |
| TC04 | Duplicate enrollment rejected | PASS |
| TC05 | Unknown student Id on enroll | PASS |
| TC06 | Course count + students by program + enrollment list | PASS |

---

### TC01 — Normal registration

- **Input:** Menu `1` → Bachelor → new Id, valid age, program, study year
- **Expected:** Student created successfully
- **Actual:** Success message and student details shown
- **Result:** **PASS**

### TC02 — Invalid age

- **Input:** Age `14`
- **Expected:** Rejected; age must be 15–65
- **Actual:** `Value must be between 15 and 65.`
- **State check:** No invalid student stored from that input alone
- **Result:** **PASS**

### TC03 — Duplicate Id

- **Input:** Register again with seed Id `S1001`
- **Expected:** Rejected as duplicate
- **Actual:** `Student Id 'S1001' already exists.`
- **Result:** **PASS**

### TC04 — Duplicate enrollment

- **Input:** Enroll `S1001` into `CS201` again
- **Expected:** Rejected
- **Actual:** `Student 'S1001' is already enrolled in course 'CS201'.`
- **Result:** **PASS**

### TC05 — Unknown Id

- **Input:** Enroll `S9999` into `CS201`
- **Expected:** Rejected as unknown student
- **Actual:** `Student 'S9999' was not found.`
- **Result:** **PASS**

### TC06 — Required displays

- **Input:** Menu `4` for `S1001`; menu `5` for `Computer Science`; menu `6` enrollment list
- **Expected:** Course count shown; program filter works; enrollment list shown
- **Actual:** All three displays work with seed data
- **Result:** **PASS**
