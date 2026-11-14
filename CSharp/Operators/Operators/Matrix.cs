class Matrix
{
    private readonly double[,] cells;

    public Matrix(int rows, int cols) =>
        cells = new double[rows, cols];

    public Matrix(double[,] values) =>
        cells = (double[,])values.Clone();

    public int Rows => cells.GetLength(0);
    public int Cols => cells.GetLength(1);

    // Індексатор з двома параметрами.
    public double this[int row, int col]
    {
        get => cells[Check(row, Rows), Check(col, Cols)];
        set => cells[Check(row, Rows), Check(col, Cols)] = value;
    }

    public static Matrix operator +(Matrix x, Matrix y)
    {
        if (x.Rows != y.Rows || x.Cols != y.Cols)
        {
            throw new ArgumentException("різні розміри матриць");
        }
        Matrix result = new(x.Rows, x.Cols);
        for (int i = 0; i < x.Rows; i++)
            for (int j = 0; j < x.Cols; j++)
                result[i, j] = x[i, j] + y[i, j];
        return result;
    }

    public static Matrix operator *(Matrix x, Matrix y)
    {
        if (x.Cols != y.Rows)
        {
            throw new ArgumentException("стовпців A ≠ рядків B");
        }
        Matrix result = new(x.Rows, y.Cols);
        for (int i = 0; i < x.Rows; i++)
            for (int j = 0; j < y.Cols; j++)
                for (int k = 0; k < x.Cols; k++)
                    result[i, j] += x[i, k] * y[k, j];
        return result;
    }

    public override string ToString()
    {
        var text = new System.Text.StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                text.Append($"{cells[i, j],6}");
            }
            text.AppendLine();
        }
        return text.ToString();
    }

    private static int Check(int index, int size) =>
        index >= 0 && index < size
            ? index
            : throw new IndexOutOfRangeException($"індекс {index}");
}
