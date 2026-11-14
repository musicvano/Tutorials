class SalariedEmployee : Employee
{
    public SalariedEmployee(string name, decimal salary) : base(name)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(salary);
        Salary = salary;
    }

    public decimal Salary { get; }

    public override decimal CalculatePay() => Salary;

    public override string Describe() => $"{base.Describe()} (оклад)";
}

