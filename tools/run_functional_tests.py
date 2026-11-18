"""Scripted console tests aligned with the slim Case 01 menu."""
# ------------------------------------------------------------
# run_functional_tests.py
# Bu dosya konsol uygulamasini otomatik girdilerle test eder.
# Amac: Brief'teki basarili/basarisiz senaryolari dogrulamak.
# ------------------------------------------------------------

from __future__ import annotations

import subprocess
import sys
from pathlib import Path

# ROOT: Proje kok klasoru (tools klasorunun bir ustu)
ROOT = Path(__file__).resolve().parents[1]

# DOTNET: Windows'ta dotnet.exe'nin tipik yolu
DOTNET = r"C:\Program Files\dotnet\dotnet.exe"

# PROJ: Derlenecek/csproj dosyasinin yolu
PROJ = ROOT / "StudentManagementSystem.csproj"

# OUT: Ham konsol ciktisinin kaydedilecegi dosya
OUT = ROOT / "docs" / "TEST_EVIDENCE.txt"

# SUMMARY: PASS/FAIL ozetinin kaydedilecegi dosya
SUMMARY = ROOT / "docs" / "TEST_RUN_SUMMARY.txt"


def main() -> int:
    # Once projeyi derler; derleme basarisizsa testlere gecmez
    build = subprocess.run([DOTNET, "build", str(PROJ), "-nologo", "-v", "q"], cwd=str(ROOT), capture_output=True, text=True)
    if build.returncode != 0:
        print(build.stdout)
        print(build.stderr)
        return 1

    # script: Menuye sirayla verilecek otomatik girdiler
    # Menu: 1 register, 2 course, 3 enroll, 4 count, 5 by program, 6 enrollments, 7 students, 0 exit
    script = [
        "1", "B", "S3001", "Marie Curie", "22", "Physics", "2", "",          # TC01 normal kayit
        "1", "B", "S3003", "Bad Age", "14", "20", "Mathematics", "1", "",    # TC02 gecersiz yas sonra duzeltme
        "1", "B", "S1001", "Dup", "20", "Computer Science", "2", "",         # TC03 yinelenen Id
        "3", "S1001", "CS201", "",                                           # TC04 duplicate enrollment
        "3", "S9999", "CS201", "",                                           # TC05 bilinmeyen Id
        "4", "S1001", "",                                                    # TC06 ders sayisi
        "5", "Computer Science", "",                                         # TC06 programa gore liste
        "6", "",                                                             # TC06 enrollment listesi
        "0",                                                                 # cikis
    ]

    # Uygulamayi calistirip stdin'e script'i gonderir, stdout'u yakalar
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

    # Konsol cikisini tek metinde birlestirir
    output = (proc.stdout or "") + ("\n" + proc.stderr if proc.stderr else "")

    # Ham kaniti dosyaya yazar
    OUT.write_text(output, encoding="utf-8")

    # checks: Her test senaryosu icin beklenen metin var mi diye bakar
    checks = {
        "TC01_register": "S3001" in output or "Marie Curie" in output,
        "TC02_invalid_age": "Value must be between 15 and 65" in output,
        "TC03_dup_id": "already exists" in output,
        "TC04_dup_enroll": "already enrolled" in output,
        "TC05_unknown": "was not found" in output,
        "TC06_count_program_list": ("Number of courses" in output) and ("Computer Science" in output),
    }

    # Ozet satirlarini olusturur
    lines = [f"{k}: {'PASS' if v else 'FAIL'}" for k, v in checks.items()]
    overall = all(checks.values())
    lines.append(f"OVERALL: {'PASS' if overall else 'FAIL'}")

    # Ozeti kaydeder ve ekrana basar
    SUMMARY.write_text("\n".join(lines) + "\n", encoding="utf-8")
    print("\n".join(lines))

    # Hepsi gectiyse 0, aksi halde 2 doner
    return 0 if overall else 2


if __name__ == "__main__":
    # Betik dogrudan calistirildiginda main'i cagirir
    sys.exit(main())
