using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PixelReshaper
{
    class Program
    {
        const int width = 128; //make customizable
        const int height = 128;

        static void Main(string[] args)
        {
            Program program = new Program();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            GeneratedImage gimage = new GeneratedImage(width, height);
            gimage.GenerateNoise(4);
            //Image image = Bitmap.FromFile("birdboy.jpg");
            //GeneratedImage gimage = new GeneratedImage(image);

            gimage.Iterate(8, 5);
            gimage.Save();

            timer.Stop();
            Console.WriteLine("Done in " + timer.Elapsed + "!");

            //Save(bitmap); //final save

        }



    }

    class GeneratedImage
    {
        private readonly string fileName;
        private readonly int width;
        private readonly int height;
        private Bitmap bitmap;
        private Dictionary<Coordinate, Color> map = new Dictionary<Coordinate, Color>();

        public GeneratedImage(int width, int height, string fileName = "Image")
        {
            this.width = width;
            this.height = height;
            this.fileName = fileName;
            bitmap = new Bitmap(width, height);

            UpdateDictionary();
        }

        public GeneratedImage(string fileLoad)
        {
            bitmap = (Bitmap)Bitmap.FromFile(fileLoad);
            width = bitmap.Width;
            height = bitmap.Height;
            fileName = "Converted_" + fileLoad;

            UpdateDictionary();
        }

        public GeneratedImage(Image image)
        {
            bitmap = (Bitmap)image;

            width = bitmap.Width;
            height = bitmap.Height;
            fileName = "Converted_" + image.Size;

            UpdateDictionary();
        }

        public void GenerateNoise(int amountOfColors)
        {
            Random random = new Random();

            Color[] randomColors = new Color[amountOfColors];
            for (int i = 0; i < amountOfColors; i++)
            {
                randomColors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                Console.WriteLine(randomColors[i]);
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = randomColors[random.Next(amountOfColors)];
                    bitmap.SetPixel(x, y, color);
                }
            }

            //Save in noise-stage
            bitmap.Save(fileName + "_Generated_Noise", ImageFormat.Png);

            UpdateDictionary();
        }

        public void Iterate(int iterations, int matrixRadius, bool texture = false)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Coordinate current = new Coordinate(x, y);
                        Color color = FindNeighbors(current, matrixRadius); //Set most common neighbor to color

                        bitmap.SetPixel(x, y, color);
                    }
                }

                UpdateDictionary();
            }
        }

        private void UpdateDictionary()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Coordinate co = new Coordinate(x, y);
                    map[co] = bitmap.GetPixel(x, y);
                }
            }
        }

        private Color FindNeighbors(Coordinate point, int radius, bool texture = false) //returns most common neighbor
        {
            Color result = map[point];
            Dictionary<Color, int> occurances = new Dictionary<Color, int>();

            //Get neighbors
            Coordinate start = new Coordinate(point.x - radius, point.y - radius);
            Coordinate end = new Coordinate(point.x + radius, point.y + radius);

            for (int x = start.x; x < end.x; x++)
            {
                for (int y = start.y; y < end.y; y++)
                {
                    Coordinate current = new Coordinate(x, y);
                    if (current.x >= 0 && current.y >= 0 && current.x <= width && current.y <= height && !texture)
                    {
                        Color color;
                        map.TryGetValue(current, out color);
                        if (!(current.x == point.x && current.y == point.y))
                        {
                            if (occurances.ContainsKey(color))
                            {
                                occurances[color]++;
                            }
                            else
                            {
                                occurances.Add(color, 1);
                            }
                        }
                    }
                }
            }

            if (occurances.Count > 0)
            {
                result = occurances.Aggregate((i, j) => i.Value > j.Value ? i : j).Key;
            }

            return result;
        }

        public void Save()
        {
            bitmap.Save(fileName, ImageFormat.Png);
        }

        public struct Coordinate
        {
            public readonly int x;
            public readonly int y;
            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
