import numpy as np
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt

def main() -> None:
    hours = np.array([8, 10, 12, 14])
    data = np.array([[18., 19.], [20., 21.],
                     [22., 23.], [21., 22.]])
    fig, axes = plt.subplots(1, 2, figsize=(8, 3.2),
                             layout="constrained")
    for index, style in enumerate(["o-", "s--"]):
        axes[0].plot(hours, data[:, index], style,
                     color="black", label=f"Датчик {index + 1}")
    axes[0].set(xlabel="Година", ylabel="Температура, °C")
    axes[0].legend()
    axes[0].grid(alpha=0.3)
    axes[1].bar(["1", "2"], data.mean(axis=0),
                color="0.8", edgecolor="black", hatch="//")
    axes[1].set(xlabel="Датчик", ylabel="Середня, °C", ylim=(0, 25))
    fig.savefig("sensor-report.png", dpi=200)
    plt.close(fig)
    print("Збережено sensor-report.png")

if __name__ == "__main__":
    main()
