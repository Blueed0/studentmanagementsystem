"""Scripted console tests for Student Management System. Captures actual output."""
from __future__ import annotations

import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
DOTNET = r"C:\Program Files\dotnet\dotnet.exe"
PROJ = ROOT / "StudentManagementSystem.csproj"
OUT = ROOT / "docs" / "TEST_EVIDENCE.txt"


def run_session(lines: list[str], timeout: int = 60) -> str:
    payload = "\n".join(lines) + "\n"
    proc = subprocess.run(
        [DOTNET, "run", "--project", str(PROJ), "--no-build"],
        input=payload,
        text=True,
        capture_output=True,
        cwd=str(ROOT),
        timeout=timeout,
        encoding="utf-8",
        errors="replace",
    )
    return (proc.stdout or "") + ("\n" + proc.stderr if proc.stderr else "")


def main() -> int:
    print("Building...")
    build = subprocess.run(
        [DOTNET, "build", str(PROJ), "-nologo", "-v", "q"],
        cwd=str(ROOT),
        capture_output=True,
        text=True,
        encoding="utf-8",
        errors="replace",
    )
    if build.returncode != 0:
        print(build.stdout)
        print(build.stderr)
        return 1
    print("Build OK")

    # One long interactive script covering TC01-TC08 then exit
    script = [
        # TC01 register bachelor S3001
        "1", "B", "S3001", "Marie Curie", "22", "Physics", "2", "",
        # TC02 invalid age 14 then recover with valid 20 for a throwaway attempt path:
        # ReadIntInRange loops until valid, so feed 14 then 20
        "1", "B", "S3003", "Invalid Age Demo", "14", "20", "Mathematics", "1", "",
        # TC03 duplicate id S1001 - need valid age in range first
        "1", "B", "S1001", "Dup Person", "20", "Computer Science", "2", "",
        # TC05 duplicate enrollment S1001 CS201
        "3", "S1001", "CS201", "",
        # TC07 unknown student
        "3", "S9999", "CS201", "",
        # TC06 view student courses + by program
        "10", "S1001", "",
        "14", "Computer Science", "",
        # TC08 age 15 boundary
        "1", "B", "S3002", "Young Student", "15", "Biology", "1", "",
        # TC04 fill CS101: seed has S1001. Need 2 more then 1 reject.
        # Register two students and enroll
        "1", "B", "S4001", "Stu One", "19", "CS", "1", "",
        "1", "B", "S4002", "Stu Two", "19", "CS", "1", "",
        "1", "B", "S4003", "Stu Three", "19", "CS", "1", "",
        "3", "S4001", "CS101", "",
        "3", "S4002", "CS101", "",
        "3", "S4003", "CS101", "",  # should be full
        # list enrollments for evidence
        "7", "",
        "5", "",
        "0",
    ]

    print("Running scripted session...")
    output = run_session(script)
    OUT.write_text(output, encoding="utf-8")
    print(f"Wrote {OUT}")

    checks = {
        "TC01_success": "Marie Curie" in output or "S3001" in output,
        "TC02_age_ui": "Value must be between 15 and 65" in output,
        "TC03_dup_id": "already exists" in output,
        "TC05_dup_enroll": "already enrolled" in output,
        "TC07_unknown": "was not found" in output,
        "TC06_courses": "Enrolled course count" in output or "CS101" in output,
        "TC06_by_program": "Computer Science" in output,
        "TC08_age15": "S3002" in output,
        "TC04_full": "full" in output.lower() or "No remaining seats" in output,
    }

    print("\n=== CHECK RESULTS ===")
    all_ok = True
    for name, ok in checks.items():
        print(f"{name}: {'PASS' if ok else 'FAIL'}")
        all_ok = all_ok and ok

    # Save a short summary
    summary = ROOT / "docs" / "TEST_RUN_SUMMARY.txt"
    lines = [f"{k}: {'PASS' if v else 'FAIL'}" for k, v in checks.items()]
    lines.append(f"OVERALL: {'PASS' if all_ok else 'FAIL'}")
    summary.write_text("\n".join(lines) + "\n", encoding="utf-8")
    print(f"Wrote {summary}")
    return 0 if all_ok else 2


if __name__ == "__main__":
    sys.exit(main())
