def ranking(
    journal: dict[str, list[int]],
) -> list[tuple[str, float]]:
    result: list[tuple[str, float]] = []
    for name, marks in journal.items():
        if any(mark < 0 or mark > 100 for mark in marks):
            raise ValueError("Оцінка поза межами 0..100")
        if marks:
            result.append((name, sum(marks) / len(marks)))
    return sorted(result, key=lambda item: (-item[1], item[0]))

journal = {"Олег": [80, 90], "Анна": [90, 80], "Іра": []}
for number, (name, average) in enumerate(ranking(journal), 1):
    print(f"{number}. {name:<8} {average:6.2f}")
missing = sorted(name for name, marks in journal.items()
                 if not marks)
print("Без оцінок:", ", ".join(missing))
