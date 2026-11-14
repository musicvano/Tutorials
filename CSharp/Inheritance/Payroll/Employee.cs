class Employee
{
    public Employee(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }

    public string Name { get; }

    public virtual decimal CalculatePay() => 0m;

    public virtual string Describe() => Name;
}

