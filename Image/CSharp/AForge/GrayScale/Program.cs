using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;

namespace GrayScale
{
    class Program
    {
        static void Main(string[] args)
        {
            // 入力画像
            var img = new Bitmap(@"src.jpg");
            // グレースケール化
            var gray = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721).Apply(img);
            gray.Save(@"dst.jpg");
            gray.Dispose();
        }
    }
}
