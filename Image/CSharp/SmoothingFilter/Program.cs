using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SmoothingFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = LoadImageGray("src.jpg");
            // フィルタ用のマスク
            const int maskSize = 3;
            double[,] mask = new double[maskSize, maskSize]{
                                {1/9.0,1/9.0,1/9.0},
                                {1/9.0,1/9.0,1/9.0},
                                {1/9.0,1/9.0,1/9.0}};

            // フィルタ処理
            byte[,] img2 = Filter(img, mask);
            // 画像保存
            SaveImage(img2, "dst.jpg");
        }

        // 画像をグレースケール変換して読み込み
        static byte[,] LoadImageGray(string filename)
        {
            Bitmap img = new Bitmap(filename);
            int w = img.Width;
            int h = img.Height;
            byte[,] dst = new byte[w, h];

            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    // グレイスケールに変換
                    dst[j, i] = (byte)((img.GetPixel(j, i).R + img.GetPixel(j, i).B + img.GetPixel(j, i).G) / 3);
                }
            }
            return dst;
        }

        static void SaveImage(byte[,] src, string filename)
        {
            // 画像データの幅と高さを取得
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            Bitmap img = new Bitmap(w, h);
            // ピクセル値のセット
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    img.SetPixel(j, i, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }

            // 画像の保存
            img.Save(filename);
        }
        static byte[,] Filter(byte[,] src, double[,] mask)
        {
                // 縦横サイズを配列から読み取り
                int w = src.GetLength(0);
                int h = src.GetLength(1);
                // マスクサイズの取得
                int masksize = mask.GetLength(0);
                // 出力画像用の配列
                byte[,] dst = new byte[w, h];

                // 画像処理
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        double sum = 0;
                        for (int k = -masksize / 2; k <= masksize / 2; k++)
                        {
                            for (int n = -masksize / 2; n <= masksize / 2; n++)
                            {
                                if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                                {
                                    sum += src[j + n, i + k] * mask[n + masksize / 2, k + masksize / 2];
                                }
                            }
                        }

                        dst[j, i] = Byte2Int(sum);
                    }
                }
                return dst;
        }

        // double型をbyte型に変換
        static byte Byte2Int(double num)
        {
            if (num > 255.0) return 255;
            else if (num < 0) return 0;
            else return (byte)num;
        }

    }
}
