using System;
using System.Drawing;

namespace GameJamFeb4.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Color[] colors = { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Cyan, Color.White };
            GeneratedImage g = new GeneratedImage("generated", colors, 100);

            Bitmap testmap = g.Generate(10);

            //g.Iterate(4, true);
            /*Color[] nine = g.GetMatrix(-1, -1, testmap, 3);
            
            foreach(Color c in nine){
                Console.WriteLine(c);
            }*/
            /*
            ProbabilityCalculator<int> calc = new ProbabilityCalculator<int>();
            int[] ints = { 1, 2, 3, 4, 5 };
            Console.WriteLine(calc.Calculate(ints, 10));*/
        }
    }
}
