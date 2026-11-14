"""Діагностика вибраного інтерпретатора."""
import platform
import sys

print("Python:", platform.python_version())
print("Система:", platform.system())
print("Інтерпретатор:", sys.executable)
print("Середовище:", sys.prefix != sys.base_prefix)
