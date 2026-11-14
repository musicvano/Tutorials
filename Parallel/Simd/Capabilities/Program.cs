using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine($"{RuntimeInformation.OSArchitecture}, " +
    $"{RuntimeInformation.FrameworkDescription}, логічних " +
    $"процесорів: {Environment.ProcessorCount}");

Console.WriteLine("Апаратно-незалежні вектори:");
Show("Vector.IsHardwareAccelerated", Vector.IsHardwareAccelerated);
Show("Vector<float>.Count", Vector<float>.Count);
Show("Vector<double>.Count", Vector<double>.Count);
Show("Vector128.IsHardwareAccelerated",
    Vector128.IsHardwareAccelerated);
Show("Vector256.IsHardwareAccelerated",
    Vector256.IsHardwareAccelerated);
Show("Vector512.IsHardwareAccelerated",
    Vector512.IsHardwareAccelerated);

Console.WriteLine("Набори інструкцій x86/x64:");
Show("Avx2", Avx2.IsSupported);
Show("Fma", Fma.IsSupported);
Show("Avx512F", Avx512F.IsSupported);
Show("Avx10v1", Avx10v1.IsSupported);
Show("Avx10v2", Avx10v2.IsSupported);
Console.WriteLine("Набори інструкцій Arm64:");
Show("AdvSimd (NEON)", AdvSimd.IsSupported);

static void Show(string name, object value)
{
    string text = value is bool b ? (b ? "так" : "ні") : $"{value}";
    Console.WriteLine($"  {name,-32} {text}");
}
