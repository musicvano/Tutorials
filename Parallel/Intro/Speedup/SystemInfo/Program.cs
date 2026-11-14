using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;

Console.OutputEncoding = System.Text.Encoding.UTF8;

GCMemoryInfo memory = GC.GetGCMemoryInfo();
double gb = memory.TotalAvailableMemoryBytes / Math.Pow(1024, 3);

Print("Операційна система", RuntimeInformation.OSDescription);
Print("Архітектура ОС", RuntimeInformation.OSArchitecture);
Print("Архітектура процесу", RuntimeInformation.ProcessArchitecture);
Print("64-розрядний процес", Environment.Is64BitProcess);
Print("Середовище .NET", RuntimeInformation.FrameworkDescription);
Print("Ідентифікатор RID", RuntimeInformation.RuntimeIdentifier);
Print("Логічних процесорів", Environment.ProcessorCount);
Print("Доступна пам’ять, ГБ", $"{gb:F1}");
Print("Серверний збирач сміття", GCSettings.IsServerGC);
Print("Таймер високої точності", Stopwatch.IsHighResolution);
Print("Тактів Stopwatch за 1 с", $"{Stopwatch.Frequency:N0}");

#if DEBUG
Print("Конфігурація", "Debug");
#else
Print("Конфігурація", "Release");
#endif

static void Print(string name, object value) =>
    Console.WriteLine($"{name,-26}{value}");
