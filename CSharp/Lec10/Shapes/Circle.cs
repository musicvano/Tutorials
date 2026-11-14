class Circle(string name, double radius) : Shape(name)
{
    public override string Kind => "circle";
    public override double Area() => Math.PI * radius * radius;
}
