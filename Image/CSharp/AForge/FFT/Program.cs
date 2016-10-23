using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 画像の取得
            var img = new Bitmap(@"src.jpg");
            // 画像の解像度が2のべき乗になるように調整
            var bitWidth = Convert.ToString(img.Width - 1, 2).Length;
            var bitHeight = Convert.ToString(img.Height - 1, 2).Length;
            var width = (int)Math.Pow(2, bitWidth);
            var height = (int)Math.Pow(2, bitHeight);
            // 隙間の部分はゼロ埋め
            var imgPadded = new Bitmap(width, height, img.PixelFormat);
            var graphics = Graphics.FromImage(imgPadded);
            graphics.DrawImage(img, 0, 0);
            graphics.Dispose();
            // グレースケール化
            var gray = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721).Apply(imgPadded);
            // 高速フーリエ変換
            var complex = AForge.Imaging.ComplexImage.FromBitmap(gray);
            complex.ForwardFourierTransform();
            // 保存
            Bitmap img2 = complex.ToBitmap();
            img2.Save(@"dst.jpg");
            img2.Dispose();
        }
    }
}
