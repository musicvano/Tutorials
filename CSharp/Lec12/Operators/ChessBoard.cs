class ChessBoard
{
    private readonly char[,] cells = new char[8, 8];

    public ChessBoard()
    {
        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                cells[r, c] = '.';
    }

    // Індексатор за номерами: рядок 0 – горизонталь 1.
    public char this[int row, int col]
    {
        get => cells[row, col];
        set => cells[row, col] = value;
    }

    // Рядковий індексатор за шаховою нотацією.
    public char this[string square]
    {
        get
        {
            var (row, col) = Parse(square);
            return cells[row, col];
        }
        set
        {
            var (row, col) = Parse(square);
            cells[row, col] = value;
        }
    }

    public void Move(string from, string to)
    {
        this[to] = this[from];
        this[from] = '.';
    }

    public override string ToString()
    {
        var text = new System.Text.StringBuilder();
        for (int r = 7; r >= 0; r--)
        {
            text.Append($"{r + 1} ");
            for (int c = 0; c < 8; c++)
            {
                text.Append(cells[r, c]).Append(' ');
            }
            text.AppendLine();
        }
        return text.Append("  a b c d e f g h").ToString();
    }

    private static (int Row, int Col) Parse(string square)
    {
        if (square is not [var file, var rank])
        {
            throw new ArgumentException("потрібно поле, як e4");
        }
        int col = char.ToLower(file) - 'a';
        int row = rank - '1';
        if (col is < 0 or > 7 || row is < 0 or > 7)
        {
            throw new ArgumentException("поле поза дошкою");
        }
        return (row, col);
    }
}
