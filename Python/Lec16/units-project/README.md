# units-converter

A teaching converter of a non-negative finite length from metres to centimetres
(from 0 to 1000000 m). The `units_converter` package provides the
`to_centimetres` function and the `units-convert` console command.

Requirements: Python 3.14 or newer, no runtime dependencies.

Installing the built wheel:

```powershell
python -m pip install units_converter-0.1.0-py3-none-any.whl
```

Example:

```powershell
units-convert 1.25
```

Output: `125.00 см`.
