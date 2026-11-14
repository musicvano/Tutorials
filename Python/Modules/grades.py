import csv
from pathlib import Path
from tempfile import TemporaryDirectory


with TemporaryDirectory() as folder:
    path = Path(folder) / "grades.csv"
    with path.open("w", encoding="utf-8-sig", newline="") as f:
        writer = csv.DictWriter(f, fieldnames=["name", "score"],
                                delimiter=";")
        writer.writeheader()
        writer.writerows([{"name": "Олена", "score": 80},
                          {"name": "Іван", "score": 90}])
    scores: list[int] = []
    with path.open(encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f, delimiter=";")
        if reader.fieldnames != ["name", "score"]:
            raise ValueError("неправильний заголовок")
        for row in reader:
            if None in row or not row["name"].strip():
                raise ValueError("неправильний рядок")
            score = int(row["score"])
            if not 0 <= score <= 100:
                raise ValueError("бал поза межами")
            scores.append(score)
    if not scores:
        raise ValueError("немає оцінок")
    print(f"Середній бал: {sum(scores) / len(scores):.2f}")
