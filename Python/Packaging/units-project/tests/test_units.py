import pytest

from units_converter import to_centimetres


def test_conversion() -> None:
    assert to_centimetres(1.25) == pytest.approx(125)
    assert to_centimetres(0) == 0


@pytest.mark.parametrize("value", [-1, float("nan"), 1e7])
def test_invalid(value: float) -> None:
    with pytest.raises(ValueError):
        to_centimetres(value)
