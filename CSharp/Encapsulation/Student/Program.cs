Console.OutputEncoding = System.Text.Encoding.UTF8;

var students = new[]
{
    new Student("Koval Olena", 2026),
    new Student("Bondar Petro", 2026),
    new Student("Melnyk Iryna", 2025),
};

foreach (Student s in students)
{
    Console.WriteLine($"{s.RecordBookNumber}  {s.FullName}");
}
Console.WriteLine($"Students created: {Student.Count}");

students[1].FullName = "Bondarenko Petro";   // the name can be changed
// students[1].RecordBookNumber = "…";        // CS0200: read-only
Console.WriteLine(students[1]);

class Student
{
    private static int count;
    private string fullName = "";

    public Student(string fullName, int admissionYear)
    {
        FullName = fullName;
        count++;
        RecordBookNumber = $"{admissionYear % 100:D2}-{count:D4}";
    }

    public static int Count => count;

    // The number is assigned once and never changes.
    public string RecordBookNumber { get; }

    public string FullName
    {
        get => fullName;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            fullName = value.Trim();
        }
    }

    public override string ToString() =>
        $"{RecordBookNumber} {FullName}";
}
