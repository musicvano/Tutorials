# geometry/core.py
from math import isfinite, pi


def area(radius: float) -> float:
    if not isfinite(radius) or radius <= 0:
        raise ValueError("радіус має бути скінченним і додатним")
    result = pi * radius * radius
    if not isfinite(result):
        raise ValueError("площа завелика")
    return result
