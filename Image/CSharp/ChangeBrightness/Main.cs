using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ChangeBrightness
{
    class Program
    {
        static void Main(string[] args)
        {
            // 画像の読み込み(グレースケールに変換して)
            byte[,] data = LoadImageGray("src.jpg");

            // 明るさ変更
            byte[,] filterdata = BrightnessChange(data, 50);

            // 画像保存
            SaveImage(filterdata, "dst.jpg");
        }
        // 明るさ変更(brightを足して256以上は255、0未満は0)
        static byte[,] BrightnessChange(byte[,] data, int bright)
        {
            // 画像データの幅と高さを取得
            int w = data.GetLength(0);
            int h = data.GetLength(1);
            byte[,] brightdata = new byte[w, h];

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((int)data[j, i] + bright >= 256)
                    {
                        brightdata[j, i] = 255;
                    }
                    else if ((int)data[j, i] + bright < 0)
                    {
                        brightdata[j, i] = 0;
                    }
                    else
                    {
                        brightdata[j, i] = (byte)(data[j, i] + bright);
                    }
                }
            }
            return brightdata;
        }

        // 画像をグレースケール変換して読み込み
        static byte[,] LoadImageGray(string filename)
        {
            Bitmap bitmap = new Bitmap(filename);
            int w = bitmap.Width;
            int h = bitmap.Height;
            byte[,] data = new byte[w, h];
            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                 // グレイスケールに変換
                     data[j, i] = (byte)((bitmap.GetPixel(j, i).R + bitmap.GetPixel(j, i).B + bitmap.GetPixel(j, i).G) / 3);
                }
            }
            return data;
        }

        static void SaveImage(byte[,] data, string filename)
        {
            // 画像データの幅と高さを取得
            int w = data.GetLength(0);
            int h = data.GetLength(1);
            Bitmap bitmap = new Bitmap(w, h);
            // ピクセル値のセット
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    bitmap.SetPixel(j,i,Color.FromArgb(data[j, i], data[j, i], data[j, i]));
                }
            }
            // 画像の保存
            bitmap.Save(filename);
        }
    }
}
