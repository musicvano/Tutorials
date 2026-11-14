Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.Write("Natural number: ");
if (long.TryParse(Console.ReadLine(), out long number)
    && number >= 0)
{
    int sum = DigitSum(number);
    Console.WriteLine($"Digit sum: {sum}");
    Console.WriteLine($"Digital root: {DigitalRoot(number)}");
}
else
{
    Console.WriteLine("A nonnegative integer is required.");
}

Console.Write("Phrase: ");
string phrase = Console.ReadLine() ?? "";
string letters = OnlyLetters(phrase);
string verdict = IsPalindrome(letters, 0, letters.Length - 1)
    ? "a palindrome"
    : "not a palindrome";
Console.WriteLine($"“{phrase}” — {verdict}");

// Digit sum: last digit + digit sum of the remaining number.
static int DigitSum(long n) =>
    n < 10 ? (int)n : (int)(n % 10) + DigitSum(n / 10);

// Sum digits until only one digit remains.
static int DigitalRoot(long n)
{
    int sum = DigitSum(n);
    return sum < 10 ? sum : DigitalRoot(sum);
}

// Compares the outer characters and moves inward.
static bool IsPalindrome(string s, int left, int right)
{
    if (left >= right)
    {
        return true;                      // 0 or 1 character
    }
    if (s[left] != s[right])
    {
        return false;
    }
    return IsPalindrome(s, left + 1, right - 1);
}

// Lowercase letters without spaces, punctuation, or apostrophes.
static string OnlyLetters(string text)
{
    var sb = new System.Text.StringBuilder();
    foreach (char c in text)
    {
        if (char.IsLetter(c))
        {
            sb.Append(char.ToLower(c));
        }
    }
    return sb.ToString();
}
