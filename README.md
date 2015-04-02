# Pattern
Simple Pattern Matching in C#

# How to use

Use the **Pattern** project in whichever way you are used to!

#Examples

Check out the Examples Project (Console App)

## Simple example

```C#
Match.Switch(
  When.Its<int>((x) => { Console.WriteLine("Int " + x); }),
  When.Its<float>((x) => { Console.WriteLine("Float " + x); })
).Against(10.5f);

// Float 10.5
```

## Little more complex

```C#
// You can cache the "Match" object
// And re-use to match against different objects
// Imp: Keep most generic items at the last, Poly comes after Square
var match = Match.Switch(

    When.Its<Circle>(x => { Console.WriteLine("Area of Circle is = " + 3.14 * x.Radius * x.Radius); }),

    When.Its<Square>(x => { Console.WriteLine("Area of Square is = " + x.Area()); }),

    When.Its<Polygon>(x =>
    {
        switch (x.Sides)
        {
            case 5:
                Console.WriteLine("Area of Pentagon is = " + 1.72f * x.Length * x.Length);
                break;

            case 6:
                Console.WriteLine("Area of Hexagon is = " + 2.6f * x.Length * x.Length);
                break;
        }
    }),

    When.Its<IPolygon>(x =>
    {
        try
        {
            Console.WriteLine("Area of Polygon is = " + x.Area());
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("Dunno how to calculate area");
        }
    })
);

match.Against(new Circle(10));
match.Against(new Square(5));
match.Against(new Polygon(5, 10));
match.Against(new ConcavePoly());

// Check out the example code if this is not clear enough
```

#License

MIT

