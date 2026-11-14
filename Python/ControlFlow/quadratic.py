import math

a = float(input("a: "))
b = float(input("b: "))
c = float(input("c: "))
if not (math.isfinite(a) and math.isfinite(b)
        and math.isfinite(c)):
    print("Коефіцієнти мають бути скінченними")
elif a == 0:
    if b != 0:
        print(f"Лінійне: x = {-c / b:.3f}")
    elif c == 0:
        print("Безліч розв’язків")
    else:
        print("Розв’язків немає")
else:
    d = b * b - 4 * a * c
    if not math.isfinite(d):
        print("Надто великі коефіцієнти")
    elif d < 0:
        print("Дійсних коренів немає")
    elif d == 0:
        print(f"x = {-b / (2 * a):.3f}")
    else:
        root = math.sqrt(d)
        print(f"x1 = {(-b - root) / (2 * a):.3f}")
        print(f"x2 = {(-b + root) / (2 * a):.3f}")
