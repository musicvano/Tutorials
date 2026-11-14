from collections.abc import Callable
from functools import wraps
from time import perf_counter
from typing import ParamSpec, TypeVar

P = ParamSpec("P")
R = TypeVar("R")


def retry(times: int) -> Callable[[Callable[P, R]], Callable[P, R]]:
    if times < 1:
        raise ValueError("Потрібна хоча б одна спроба")
    def decorate(func: Callable[P, R]) -> Callable[P, R]:
        @wraps(func)
        def wrapper(*args: P.args, **kwargs: P.kwargs) -> R:
            for attempt in range(1, times + 1):
                try:
                    return func(*args, **kwargs)
                except TimeoutError:
                    print("Невдала спроба:", attempt)
                    if attempt == times:
                        raise
            raise AssertionError("Недосяжна гілка")
        return wrapper
    return decorate


def timer(func: Callable[P, R]) -> Callable[P, R]:
    @wraps(func)
    def wrapper(*args: P.args, **kwargs: P.kwargs) -> R:
        started = perf_counter()
        try:
            return func(*args, **kwargs)
        finally:
            elapsed = perf_counter() - started
            print(func.__name__, "час >= 0:", elapsed >= 0)
    return wrapper


attempts = 0


@timer
@retry(times=3)
def fetch() -> str:
    global attempts
    attempts += 1
    if attempts < 3:
        raise TimeoutError("Навчальна відмова")
    return "готово"


print(fetch())
