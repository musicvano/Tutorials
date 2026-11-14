abstract class Shape
{
    protected Shape(string name) => Name = name;

    public string Name { get; }
    public abstract double Area();          // без тіла
    public abstract string Kind { get; }

    public string Describe() => $"{Kind}: {Area():F2}";
}
