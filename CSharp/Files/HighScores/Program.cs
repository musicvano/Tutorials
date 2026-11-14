Console.OutputEncoding = System.Text.Encoding.UTF8;

const int NameLength = 16;                    // characters
const int RecordSize = 4 + 4 + NameLength * 2;  // Id, Score, Name
string path = Path.Combine("data", "scores.bin");
Directory.CreateDirectory("data");

// Writing three records.
using (FileStream stream = new(path, FileMode.Create))
using (BinaryWriter writer = new(stream))
{
    WriteRecord(writer, new(1, 12_500, "Olena"));
    WriteRecord(writer, new(2, 9_870, "Petro"));
    WriteRecord(writer, new(3, 15_020, "Iryna"));
}
long length = new FileInfo(path).Length;
Console.WriteLine($"File size: {length} B = 3 × {RecordSize}");

// Random access: reading and updating a record by number.
using (FileStream stream = new(path, FileMode.Open))
using (BinaryReader reader = new(stream))
using (BinaryWriter writer = new(stream))
{
    stream.Seek(1 * RecordSize, SeekOrigin.Begin);
    ScoreRecord second = ReadRecord(reader);
    Console.WriteLine($"Record 1: {second}");

    stream.Seek(1 * RecordSize, SeekOrigin.Begin);
    WriteRecord(writer, second with { Score = 16_300 });
    Console.WriteLine($"Position after the update: {stream.Position}");

    stream.Seek(0, SeekOrigin.Begin);
    Console.WriteLine("All records:");
    while (stream.Position < stream.Length)
    {
        Console.WriteLine($"  {ReadRecord(reader)}");
    }
}

static void WriteRecord(BinaryWriter writer, ScoreRecord record)
{
    writer.Write(record.Id);
    writer.Write(record.Score);
    // A fixed-length name: padded with '\0' characters.
    string name =
        record.Name.PadRight(NameLength, '\0')[..NameLength];
    foreach (char c in name)
    {
        writer.Write((ushort)c);
    }
}

static ScoreRecord ReadRecord(BinaryReader reader)
{
    int id = reader.ReadInt32();
    int score = reader.ReadInt32();
    char[] name = new char[NameLength];
    for (int i = 0; i < NameLength; i++)
    {
        name[i] = (char)reader.ReadUInt16();
    }
    return new(id, score, new string(name).TrimEnd('\0'));
}

record ScoreRecord(int Id, int Score, string Name);
