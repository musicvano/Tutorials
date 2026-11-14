import os
import subprocess
import sys
from pathlib import Path

env = os.environ.copy()
system = Path(os.environ["SystemRoot"])
env["PATH"] = os.pathsep.join(map(str, [
    Path(sys.executable).parent, Path(sys.base_prefix),
    system / "System32", system,
]))
for key in ("QT_PLUGIN_PATH", "QT_QPA_PLATFORM_PLUGIN_PATH",
            "PYTHONPATH", "PYTHONHOME"):
    env.pop(key, None)
subprocess.run([
    sys.executable, "-m", "PyInstaller", "--clean",
    "--onedir", "--name", "BudgetClean",
    "--workpath", "build-clean", "--distpath", "dist-clean",
    "--add-data", "defaults.json:.", "budget.py",
], env=env, cwd=Path(__file__).parent, check=True)
