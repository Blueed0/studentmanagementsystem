"""Export the assessment documentation package to Word (.docx) files."""
# ------------------------------------------------------------
# export_submission_docx.py
# Bu dosya Markdown dokumanlarini Word (.docx) dosyalarina cevirir.
# Amac: Hocaya teslim edilecek Word paketini otomatik uretmek.
# ------------------------------------------------------------

from __future__ import annotations

import re
from pathlib import Path

from docx import Document
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.shared import Pt, Inches, RGBColor

# ROOT: Proje kok klasoru
ROOT = Path(__file__).resolve().parents[1]

# DOCS: Markdown dokumanlarinin bulundugu klasor
DOCS = ROOT / "docs"

# OUT_DIR: Uretilen Word dosyalarinin yazilacagi klasor
OUT_DIR = ROOT / "submission"


def add_heading(doc: Document, text: str, level: int = 1) -> None:
    # Word belgesine baslik ekler
    doc.add_heading(text, level=level)


def add_para(doc: Document, text: str, bold: bool = False) -> None:
    # Word belgesine normal paragraf ekler
    p = doc.add_paragraph()
    run = p.add_run(text)
    run.bold = bold
    run.font.size = Pt(11)
    run.font.name = "Calibri"


def add_md_file(doc: Document, path: Path, skip_mermaid: bool = True) -> None:
    # Markdown dosyasini okuyup Word icerigine donusturur
    text = path.read_text(encoding="utf-8")

    # Mermaid/kod bloklarini Word icin sade metne cevirir
    if skip_mermaid:
        text = re.sub(r"```mermaid.*?```", "[Mermaid diagram — open docs/*.md in mermaid.live or VS Code to view/export image]", text, flags=re.S)
        text = re.sub(r"```.*?```", lambda m: "\n".join("    " + ln for ln in m.group(0).strip("`").splitlines()[1:]), text, flags=re.S)

    # Tablo satirlari biriktirmek icin gecici liste
    in_table = False
    table_rows: list[list[str]] = []

    def flush_table() -> None:
        # Biriken markdown tablo satirlarini Word tablosuna yazar
        nonlocal table_rows, in_table
        if not table_rows:
            in_table = False
            return

        cols = max(len(r) for r in table_rows)
        table = doc.add_table(rows=len(table_rows), cols=cols)
        table.style = "Table Grid"

        for i, row in enumerate(table_rows):
            for j in range(cols):
                cell = table.cell(i, j)
                cell.text = row[j] if j < len(row) else ""
                # Ilk satiri baslik gibi kalin yapar
                if i == 0:
                    for p in cell.paragraphs:
                        for r in p.runs:
                            r.bold = True

        doc.add_paragraph("")
        table_rows = []
        in_table = False

    # Markdown satirlarini tek tek yorumlar
    for raw in text.splitlines():
        line = raw.rstrip()

        # Bos satir: varsa tabloyu kapatir
        if not line.strip():
            flush_table()
            continue

        # Markdown tablo satiri (| ... |)
        if line.strip().startswith("|") and "|" in line.strip()[1:]:
            # Ayirici satirlari (|---|) atlar
            if re.match(r"^\s*\|?\s*:?-{3,}", line.replace("|", " ").strip()) or set(line.replace("|", "").strip()) <= set("-: "):
                continue
            cells = [c.strip() for c in line.strip().strip("|").split("|")]
            table_rows.append(cells)
            in_table = True
            continue
        else:
            flush_table()

        # Baslik / liste / normal paragraf donusumu
        if line.startswith("# "):
            add_heading(doc, line[2:].strip(), 1)
        elif line.startswith("## "):
            add_heading(doc, line[3:].strip(), 2)
        elif line.startswith("### "):
            add_heading(doc, line[4:].strip(), 3)
        elif line.startswith("> "):
            p = doc.add_paragraph(line[2:].strip())
            p.runs[0].italic = True if p.runs else False
        elif line.startswith("- "):
            doc.add_paragraph(line[2:].strip(), style="List Bullet")
        elif re.match(r"^\d+\.\s", line):
            doc.add_paragraph(re.sub(r"^\d+\.\s", "", line), style="List Number")
        else:
            # Markdown kalin/kod isaretlerini temizler
            clean = re.sub(r"\*\*(.*?)\*\*", r"\1", line)
            clean = re.sub(r"`([^`]*)`", r"\1", clean)
            add_para(doc, clean)

    # Dosya sonunda acik kalan tabloyu kapatir
    flush_table()


def set_doc_defaults(doc: Document) -> None:
    # Sayfa kenar bosluklarini ayarlar
    section = doc.sections[0]
    section.top_margin = Inches(1)
    section.bottom_margin = Inches(1)
    section.left_margin = Inches(1)
    section.right_margin = Inches(1)


def build_report() -> Path:
    # Ana teslim raporunu (rapor + ekler) tek Word dosyasinda uretir
    doc = Document()
    set_doc_defaults(doc)

    # Kapak basligi
    title = doc.add_paragraph()
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    r = title.add_run("Object Oriented Programming — Midterm Case Study")
    r.bold = True
    r.font.size = Pt(16)

    # Yazar satiri
    sub = doc.add_paragraph()
    sub.alignment = WD_ALIGN_PARAGRAPH.CENTER
    s = sub.add_run("01. Student Management System\nAuthors: Yigit Ali Engin, Nihat Yaman Celen")
    s.font.size = Pt(12)

    # Ana rapor govdesi
    add_md_file(doc, DOCS / "REPORT.md")

    # Ek A: UML
    doc.add_page_break()
    add_heading(doc, "Appendix A — UML Class Diagram", 1)
    add_md_file(doc, DOCS / "UML_CLASS_DIAGRAM.md")

    # Ek B: Test case'ler
    doc.add_page_break()
    add_heading(doc, "Appendix B — Test Cases", 1)
    add_md_file(doc, DOCS / "TEST_CASES.md")

    # Ek C: Otomatik test ozeti (varsa)
    evidence = DOCS / "TEST_RUN_SUMMARY.txt"
    if evidence.exists():
        doc.add_page_break()
        add_heading(doc, "Appendix C — Test Run Summary", 1)
        add_para(doc, evidence.read_text(encoding="utf-8"))

    # Ek D: Ham konsol kaniti (varsa, kisaltilmis)
    raw = DOCS / "TEST_EVIDENCE.txt"
    if raw.exists():
        doc.add_page_break()
        add_heading(doc, "Appendix D — Console Test Evidence (excerpt)", 1)
        content = raw.read_text(encoding="utf-8", errors="replace")
        excerpt = content if len(content) < 12000 else content[:12000] + "\n...[truncated]..."
        for block in excerpt.splitlines():
            p = doc.add_paragraph(block)
            for run in p.runs:
                run.font.name = "Consolas"
                run.font.size = Pt(8)

    # Ek E: Katki beyani
    doc.add_page_break()
    add_heading(doc, "Appendix E — Member Contributions", 1)
    add_md_file(doc, DOCS / "MEMBER_CONTRIBUTIONS.md")

    # Ek F: AI kullanim beyani
    doc.add_page_break()
    add_heading(doc, "Appendix F — AI-Use Statement", 1)
    add_md_file(doc, DOCS / "AI_USE_STATEMENT.md")

    # Dosyayi submission klasorune kaydeder
    out = OUT_DIR / "01_Student_Management_System_REPORT.docx"
    doc.save(out)
    return out


def build_standalone(md_name: str, out_name: str) -> Path:
    # Tek bir Markdown dosyasini ayri Word dosyasina cevirir
    doc = Document()
    set_doc_defaults(doc)
    add_md_file(doc, DOCS / md_name)
    out = OUT_DIR / out_name
    doc.save(out)
    return out


def main() -> None:
    # Cikti klasorunu olusturur (yoksa)
    OUT_DIR.mkdir(parents=True, exist_ok=True)

    # Tum Word paketini uretir
    paths = [
        build_report(),
        build_standalone("TEST_CASES.md", "01_TEST_CASES.docx"),
        build_standalone("UML_CLASS_DIAGRAM.md", "01_UML_CLASS_DIAGRAM.docx"),
        build_standalone("MEMBER_CONTRIBUTIONS.md", "01_MEMBER_CONTRIBUTIONS.docx"),
        build_standalone("AI_USE_STATEMENT.md", "01_AI_USE_STATEMENT.docx"),
        build_standalone("REPORT.md", "01_REPORT_BODY.docx"),
    ]

    # README'yi de Word'e cevirir
    readme_doc = Document()
    set_doc_defaults(readme_doc)
    add_md_file(readme_doc, ROOT / "README.md")
    readme_out = OUT_DIR / "01_README.docx"
    readme_doc.save(readme_out)
    paths.append(readme_out)

    # Uretilen dosya yollarini ekrana yazar
    print("Created:")
    for p in paths:
        print(" -", p)


if __name__ == "__main__":
    # Betik dogrudan calistirildiginda main'i baslatir
    main()
