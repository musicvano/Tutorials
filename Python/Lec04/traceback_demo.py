def parse(text: str) -> int:
    return int(text)


def process(text: str) -> int:
    return parse(text)


def main() -> None:
    print(process("abc"))


main()
