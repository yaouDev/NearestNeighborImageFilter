using System;
using System.Drawing;

namespace GameJamFeb4.src
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Color[] colors = { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Cyan, Color.White };
            //Color[] colors = { Color.Black, Color.White };
            GeneratedImage g = new GeneratedImage("generated", colors, 4);


            Bitmap testmap = g.Generate(100, 100, 50, IterationMethod.scrambled);*/

            //g.Iterate(4, true);
            /*Color[] nine = g.GetMatrix(-1, -1, testmap, 3);
            
            foreach(Color c in nine){
                Console.WriteLine(c);
            }*/
            /*
            ProbabilityCalculator<int> calc = new ProbabilityCalculator<int>();
            int[] ints = { 1, 2, 3, 4, 5 };
            Console.WriteLine(calc.Calculate(ints, 10));*/

            
            Image test = Image.FromFile("insert.jpg");
            GeneratedImage g = new GeneratedImage("generated");

            g.Insert(test);
            //g.Iterate(15, 100, true, IterationMethod.scrambled);
            //g.IterateOrdered(15, 100, true, 7);
            g.IterateScrambled(15, 100, true, 7);
        }
    }
}
