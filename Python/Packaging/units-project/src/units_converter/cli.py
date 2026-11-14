import argparse

from units_converter import to_centimetres


def main() -> int:
    parser = argparse.ArgumentParser(description="Метри → см")
    parser.add_argument("metres", type=float)
    args = parser.parse_args()
    try:
        result = to_centimetres(args.metres)
    except ValueError as error:
        parser.error(str(error))
    print(f"{result:.2f} см")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
