﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace GameJamFeb4.src
{
    class GeneratedImage
    {
        /*
         * check surrounding
         * make priority of surrounding
         * iterate through priority
         * if iteration ends and color is not set, iterate through base priority list
         * ???
         * profit
         */

        /*
         * todo:
         * make a function that checks how many colors the image contains
         */

        public Bitmap image { get; private set; }
        public Color baseColor = Color.Black;
        Color[] colors;
        ProbabilityCalculator<Color> calc = new ProbabilityCalculator<Color>();
        public int iterations { get; private set; }
        public readonly string fileName;
        public readonly int priorityStrength;

        public GeneratedImage(string fileName, Color[] colors, int priorityStrength) //colors ranked in order of priority (0 being lowest);
        {
            this.fileName = fileName;
            this.colors = colors;
            this.priorityStrength = priorityStrength;
        }

        public Bitmap Generate(int timesToIterate)
        {
            image = new Bitmap(100, 100);
            iterations = 0;

            Fill(image, baseColor);
            Draw(image);
            Iterate(timesToIterate, 100, false);

            image.Save(fileName, ImageFormat.Png);
            return image;
        }

        private void Draw(Bitmap bmp) //O(N * M * O)...
        {
            Dictionary<Color, int> existingColors = new Dictionary<Color, int>();

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color currentColor = calc.Calculate(colors, priorityStrength);
                    bmp.SetPixel(x, y, currentColor); //random color

                    if (existingColors.ContainsKey(currentColor)) existingColors[currentColor]++;
                    else existingColors.Add(currentColor, 1);
                }
            }

            Console.WriteLine("Initial Iteration Color Result: ");
            existingColors.Keys.ToList().ForEach(key => Console.WriteLine(key + ": " + existingColors[key]));
        }

        public Bitmap Iterate(int timesToIterate, int oddsToBeUnchanged, bool save)
        {
            for (int i = 0; i < timesToIterate; i++)
            {
                Dictionary<Color, int> existingColors = new Dictionary<Color, int>();
                iterations++;
                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        Color currentColor = Color.Empty;

                        if (calc.YesOrNo(oddsToBeUnchanged))
                        {
                            currentColor = image.GetPixel(x, y);
                        }
                        else
                        {
                            Color[] neighbors = GetMatrix(x, y, image, 3);
                            Dictionary<Color, int> occurance = new Dictionary<Color, int>();

                            foreach (Color neighbor in neighbors)
                            {
                                if (occurance.ContainsKey(neighbor)) occurance[neighbor]++;
                                else occurance.Add(neighbor, 1);
                            }

                            currentColor = occurance.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;

                            while (currentColor.IsEmpty)
                            {
                                currentColor = neighbors[calc.PickRndIndex(neighbors)];
                            }
                        }

                        image.SetPixel(x, y, currentColor);

                        if (existingColors.ContainsKey(currentColor)) existingColors[currentColor]++;
                        else existingColors.Add(currentColor, 1);
                    }
                }
                Console.WriteLine("Iteration " + iterations + " Color Result: ");
                existingColors.Keys.ToList().ForEach(key => Console.WriteLine(key + ": " + existingColors[key]));
            }
            
            if(save) image.Save(fileName, ImageFormat.Png);

            return image;
        }

        

        private void Fill(Bitmap bmp, Color color)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    bmp.SetPixel(x, y, color);
                }
            }
        }

        public Color[] GetMatrix(int x, int y, Bitmap bmp, int range) //Gets Matrix with columns from "above"
        {
            if (range > bmp.Width || range > bmp.Height) throw new ArgumentOutOfRangeException();

            Color[] matrix = new Color[range*range];

            int startingOffset = (int)Math.Floor((double)range / 2);

            int startX = x - startingOffset;
            int startY = y - startingOffset;

            int counter = 0;

            for(int w = 0; w < range; w++)
            {
                for(int h = 0; h < range; h++)
                {
                    if ((startX + w) < 0 || (startY + w) < 0) break;
                    if ((startX + w) >= bmp.Width || (startY + h) >= bmp.Height) break;

                    try { matrix[counter] = bmp.GetPixel(startX + w, startY + h); }
                    catch (ArgumentOutOfRangeException) { }

                    counter++;
                }
            }

            return matrix;
        }
    }
}
