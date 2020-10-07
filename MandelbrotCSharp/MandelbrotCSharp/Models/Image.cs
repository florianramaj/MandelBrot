using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotCSharp.Models
{
    public class Image
    {
        public Image(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Pixels = new int[this.Width, this.Height];
             
        }

        public int [,] Pixels
        {
            get;
            set;
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
