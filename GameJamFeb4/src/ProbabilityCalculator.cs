using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameJamFeb4.src
{
    class ProbabilityCalculator<T>
    {
        Random rnd = new Random();

        public T Calculate(T[] objs, int odds) { return Calculate(objs, odds, 0); }

        public T Calculate(T[] objs, int odds, int index) //odds out of how many
        {
            //add limit to number of iterations?

            if(YesOrNo(odds)) return objs[index];
            else if (objs.Length == (index + 1)) return Calculate(objs, odds, 0);
            else { return Calculate(objs, odds, ++index); }
        }

        /*
        public T[] CalculateFromBitmap(T[] objs, Bitmap bmp)
        {

        }*/

        public bool YesOrNo(int odds) //one in odds chance
        {
            return (rnd.Next(1, odds + 1) == 1) ? true : false;
        }

        public int PickRndIndex(T[] array)
        {
            return rnd.Next(0, array.Length);
        }
    }
}
