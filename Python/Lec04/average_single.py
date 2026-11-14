def average(scores: list[int]) -> float:
    if not scores:
        raise ValueError("список балів порожній")
    total = 0
    for score in scores:
        total += score
    return total / (len(scores) - 1)  # Навмисна помилка.


scores = [80]
print(f"Середній бал: {average(scores):.2f}")
