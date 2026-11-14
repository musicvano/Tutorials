# geometry/__main__.py
import argparse
from .core import area


def main() -> None:
    parser = argparse.ArgumentParser(description="Площа круга")
    parser.add_argument("radius", type=float)
    parser.add_argument("--digits", type=int, choices=range(5),
                        default=2)
    args = parser.parse_args()
    try:
        result = area(args.radius)
    except ValueError as error:
        parser.error(str(error))
    print(f"Площа: {result:.{args.digits}f}")


if __name__ == "__main__":
    main()
