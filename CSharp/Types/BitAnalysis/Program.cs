using System.Numerics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Integer: ");
if (!int.TryParse(Console.ReadLine(), out int n))
{
    Console.WriteLine("Error: an integer of type int is required.");
    return;
}

uint bits = (uint)n;              // the same 32 bits, unsigned
bool isEven = (n & 1) == 0;
bool isPowerOfTwo = n > 0 && (n & (n - 1)) == 0;
int ones = BitOperations.PopCount(bits);

Console.WriteLine($"Binary:        {n:B32}");
Console.WriteLine($"Hexadecimal:  0x{n:X8}");
Console.WriteLine($"As uint:          {bits}");
Console.WriteLine($"Even:            {isEven}");
Console.WriteLine($"Power of two:   {isPowerOfTwo}");
Console.WriteLine($"Set bits:  {ones}");
Console.WriteLine($"Low-order byte:    {n & 0xFF}");
Console.WriteLine($"n << 1 = {n << 1}, n >> 1 = {n >> 1}");
