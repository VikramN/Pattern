/* The MIT License (MIT)

Copyright (c) Vikram (https://github.com/VikramN)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exmples
{
    class Program
    {
        /// <summary>
        /// Could be any polygon
        /// </summary>
        interface IPolygon
        {
            float Area();
        }

        /// <summary>
        /// Concave Poly
        /// </summary>
        class ConcavePoly : IPolygon
        {
            public float Area()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Circle (not polygon :P)
        /// </summary>
        class Circle
        {
            public float Radius { get; private set; }

            public Circle(int radius)
            {
                Radius = radius;
            }
        }

        /// <summary>
        /// Generic polygon
        /// </summary>
        class Polygon : IPolygon
        {
            public int Sides { get; private set; }

            public float Length { get; private set; }

            public Polygon(int sides, float length)
            {
                Sides = sides;
                Length = length;
            }

            public virtual float Area()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Square  - 4 sided poly that actually implements Area() 
        /// </summary>
        class Square : Polygon
        {
            public Square(float length) : base(4, length)
            {

            }

            public override float Area()
            {
                return Length * Length;
            }
        }

        static void Main(string[] args)
        {
            // Example 1
            Match.Switch(
                When.Its<int>((x) => { Console.WriteLine("Int " + x); }),
                When.Its<float>((x) => { Console.WriteLine("Float " + x); })
            ).Against(10.5f);

            Match.Switch(
                When.Its<int>((x) => { Console.WriteLine(" > 10"); }).And((int x) => x > 10),
                When.Its<int>((x) => { Console.WriteLine(" < 10 ");}).And((int x) => x < 10),
                When.Its<int>((x) => { Console.WriteLine(" Whoa! 10 ");}).And((int x) => x == 10)
            ).Against(9);


            // Example 2
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
                }).And<Polygon>(x => { return x.Sides <= 6; }),

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
            match.Against(new Polygon(7, 10));
            match.Against(new ConcavePoly());
            
            // Example 3 
            // Conditional Match
            // Also, different stryle => auto infer generic type 
            match = Match.Switch(
                When.Its((Tuple<int, int, int> x) => Console.WriteLine(x.Item1 + x.Item2 + x.Item3)).
                    And((Tuple<int, int, int> y) => { return y.Item1 > 10; }),

                When.Its((object x) => Console.WriteLine("Whatever"))
            );

            match.Against(new Tuple<int, int, int>(1, 2, 4));
            match.Against(new Tuple<int, int, int>(11, 2, 4));

            Console.ReadKey();
        }
    }
}
