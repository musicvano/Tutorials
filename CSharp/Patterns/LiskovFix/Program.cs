Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Before the fix (LSP violation):");
BadBird[] flock = [new BadSparrow(), new BadPenguin()];
foreach (BadBird bird in flock)
{
    try
    {
        Console.WriteLine($"  {bird.Fly(100)}");
    }
    catch (NotSupportedException e)
    {
        Console.WriteLine($"  Error: {e.Message}");
    }
}

Console.WriteLine("After the fix (ISP + LSP):");
Bird[] birds = [new Sparrow(), new Penguin(), new Duck()];
foreach (Bird bird in birds)
{
    Console.WriteLine($"  {bird.Name}: {bird.Move()}");
}
foreach (IFlyingBird flyer in birds.OfType<IFlyingBird>())
{
    Console.WriteLine($"  in the sky: {flyer.Fly(100)}");
}

// Bad: the base class promises flight that not everyone can perform.
abstract class BadBird
{
    public abstract string Fly(int meters);
}

class BadSparrow : BadBird
{
    public override string Fly(int meters) =>
        $"the sparrow flies {meters} m";
}

class BadPenguin : BadBird
{
    public override string Fly(int meters) =>
        throw new NotSupportedException("a penguin cannot fly");
}

// Good: what is shared goes in the base class, abilities go in interfaces.
abstract class Bird
{
    public abstract string Name { get; }
    public abstract string Move();
}

interface IFlyingBird
{
    string Fly(int meters);
}

interface ISwimmingBird
{
    string Swim(int meters);
}

class Sparrow : Bird, IFlyingBird
{
    public override string Name => "sparrow";
    public override string Move() => Fly(10);
    public string Fly(int meters) => $"flies {meters} m";
}

class Penguin : Bird, ISwimmingBird
{
    public override string Name => "penguin";
    public override string Move() => Swim(10);
    public string Swim(int meters) => $"swims {meters} m";
}

class Duck : Bird, IFlyingBird, ISwimmingBird
{
    public override string Name => "duck";
    public override string Move() => Swim(5);
    public string Fly(int meters) => $"the duck flies {meters} m";
    public string Swim(int meters) => $"swims {meters} m";
}
