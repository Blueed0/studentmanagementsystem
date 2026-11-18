"""Scripted console tests aligned with the slim Case 01 menu."""
from __future__ import annotations

import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
DOTNET = r"C:\Program Files\dotnet\dotnet.exe"
PROJ = ROOT / "StudentManagementSystem.csproj"
OUT = ROOT / "docs" / "TEST_EVIDENCE.txt"
SUMMARY = ROOT / "docs" / "TEST_RUN_SUMMARY.txt"


def main() -> int:
    build = subprocess.run([DOTNET, "build", str(PROJ), "-nologo", "-v", "q"], cwd=str(ROOT), capture_output=True, text=True)
    if build.returncode != 0:
        print(build.stdout)
        print(build.stderr)
        return 1

    # Menu: 1 register, 2 course, 3 enroll, 4 count, 5 by program, 6 enrollments, 7 students, 0 exit
    script = [
        "1", "B", "S3001", "Marie Curie", "22", "Physics", "2", "",
        "1", "B", "S3003", "Bad Age", "14", "20", "Mathematics", "1", "",
        "1", "B", "S1001", "Dup", "20", "Computer Science", "2", "",
        "3", "S1001", "CS201", "",
        "3", "S9999", "CS201", "",
        "4", "S1001", "",
        "5", "Computer Science", "",
        "6", "",
        "0",
    ]

    proc = subprocess.run(
        [DOTNET, "run", "--project", str(PROJ), "--no-build"],
        input="\n".join(script) + "\n",
        text=True,
        capture_output=True,
        cwd=str(ROOT),
        timeout=60,
        encoding="utf-8",
        errors="replace",
    )
    output = (proc.stdout or "") + ("\n" + proc.stderr if proc.stderr else "")
    OUT.write_text(output, encoding="utf-8")

    checks = {
        "TC01_register": "S3001" in output or "Marie Curie" in output,
        "TC02_invalid_age": "Value must be between 15 and 65" in output,
        "TC03_dup_id": "already exists" in output,
        "TC04_dup_enroll": "already enrolled" in output,
        "TC05_unknown": "was not found" in output,
        "TC06_count_program_list": ("Number of courses" in output) and ("Computer Science" in output),
    }

    lines = [f"{k}: {'PASS' if v else 'FAIL'}" for k, v in checks.items()]
    overall = all(checks.values())
    lines.append(f"OVERALL: {'PASS' if overall else 'FAIL'}")
    SUMMARY.write_text("\n".join(lines) + "\n", encoding="utf-8")
    print("\n".join(lines))
    return 0 if overall else 2


if __name__ == "__main__":
    sys.exit(main())
