Console.OutputEncoding = System.Text.Encoding.UTF8;

Animal[] zoo =
    [new Dog("Brovko"), new Cat("Murka"), new Puppy("Rex")];

foreach (Animal a in zoo)
{
    Console.WriteLine($"{a.GetType().Name,-6} {a.Name}: {a.Speak()}");
}

int dogs = 0;
foreach (Animal a in zoo)
{
    if (a is Dog)            // a Puppy is also a Dog
    {
        dogs++;
    }
}
Console.WriteLine($"Dogs: {dogs}");

class Animal
{
    public Animal(string name)
    {
        Console.WriteLine($"  Animal({name}) constructor");
        Name = name;
    }

    public string Name { get; }

    public virtual string Speak() => "...";
}

class Dog : Animal
{
    public Dog(string name) : base(name)
    {
        Console.WriteLine($"  Dog({name}) constructor");
    }

    public override string Speak() => "Woof!";
}

// A puppy is a dog; this class cannot be inherited from.
sealed class Puppy : Dog
{
    public Puppy(string name) : base(name)
    {
        Console.WriteLine($"  Puppy({name}) constructor");
    }

    public override string Speak() => base.Speak() + " (squeaky)";
}

class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public sealed override string Speak() => "Meow!";
}
