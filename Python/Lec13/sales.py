from io import StringIO
import pandas as pd

def main() -> None:
    source = ("category,qty,price\n"
              "paper,2,2500\npen,3,1000\npaper,bad,1200\n")
    frame = pd.read_csv(StringIO(source), dtype="string")
    qty = pd.to_numeric(frame["qty"], errors="coerce")
    price = pd.to_numeric(frame["price"], errors="coerce")
    valid = (qty.between(1, 1000) & price.between(0, 100000)
             & qty.mod(1).eq(0) & price.mod(1).eq(0)
             & frame["category"].str.strip().ne(""))
    valid = valid.fillna(False)
    clean = frame.loc[valid].copy()
    clean["total"] = (qty[valid] * price[valid]).astype("int64")
    clean["category"] = clean["category"].str.strip()
    report = clean.groupby("category")["total"].sum()
    for category, cents in report.items():
        print(f"{category}: {cents // 100},{cents % 100:02d}")
    print(f"Відхилено: {int((~valid).sum())}")

if __name__ == "__main__":
    main()
