using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SumBenchmarks>();

// Сума мільйона float чотирма способами (приклад «Сума елементів
// масиву»); DisassemblyDiagnoser зберігає машинний код методів.
[DisassemblyDiagnoser(maxDepth: 1)]
public class SumBenchmarks
{
    float[] data = [];

    [Params(1_000_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        data = new float[N];
        Random random = new(42);
        for (int i = 0; i < N; i++) data[i] = random.NextSingle();
    }

    [Benchmark(Baseline = true)]
    public float Scalar()
    {
        float[] x = data;          // локальна копія: без перевірок меж
        float sum = 0;
        for (int i = 0; i < x.Length; i++) sum += x[i];
        return sum;
    }

    [Benchmark]
    public float VectorT()
    {
        float[] x = data;          // локальна копія: без перевірок меж
        Vector<float> acc = Vector<float>.Zero;
        int i = 0;
        int count = Vector<float>.Count;
        for (; i <= x.Length - count; i += count)
        {
            acc += new Vector<float>(x.AsSpan(i));
        }
        float sum = Vector.Sum(acc);
        for (; i < x.Length; i++) sum += x[i];     // хвіст
        return sum;
    }

    [Benchmark]
    public float Vector256Sum()
    {
        float[] x = data;          // локальна копія: без перевірок меж
        if (!Vector256.IsHardwareAccelerated) return Scalar();
        ref float p = ref MemoryMarshal.GetArrayDataReference(x);
        Vector256<float> acc = Vector256<float>.Zero;
        nuint i = 0, n = (nuint)x.Length;
        nuint count = (nuint)Vector256<float>.Count;     // 8
        for (; i + count <= n; i += count)
        {
            acc += Vector256.LoadUnsafe(ref p, i);
        }
        float sum = Vector256.Sum(acc);
        for (; i < n; i++) sum += x[i];            // хвіст
        return sum;
    }

    [Benchmark]
    public float Tensor() => TensorPrimitives.Sum(data);
}
