from math import isfinite


def to_centimetres(metres: float) -> float:
    if not isfinite(metres) or not 0 <= metres <= 1e6:
        raise ValueError("Довжина має бути від 0 до 1000000 м")
    return metres * 100
