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

            // サイズ取得
            int w = img.GetLength(0);
            int h = img.GetLength(1);

            // double型配列にする
            double[,] data = new double[w, h];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    data[j, i] = (double)img[j, i];
                }
            }

            double[,] re;   // 実数
            double[,] im = new double[w, h];    // 虚数

            // 2次元フーリエ変換
            FFT2D(data, im, out re, out im, w, h);

            // 実数と虚数のパワースペクトルを計算
            double[,] img2 = Spectrum(re, im, w, h);

            // 画像保存
            SaveDouble2Image(img2, "dst.jpg");

        }
        // 高速フーリエ変換
        public static void FFT(double[] re1, double[] im1, out double[] re2, out double[] im2, int bitSize)
        {
            int dataSize = 1 << bitSize;
            int[] reverseBitArray = BitScrollArray(dataSize);

            re2 = new double[dataSize];
            im2 = new double[dataSize];

            // バタフライ演算のための置き換え
            for (int i = 0; i < dataSize; i++)
            {
                re2[i] = re1[reverseBitArray[i]];
                im2[i] = im1[reverseBitArray[i]];
            }

            // バタフライ演算
            for (int stage = 1; stage <= bitSize; stage++)
            {
                int butterflyDistance = 1 << stage;
                int numType = butterflyDistance >> 1;
                int butterflySize = butterflyDistance >> 1;
                double wRe = 1.0;
                double wIm = 0.0;
                double uRe = System.Math.Cos(System.Math.PI / butterflySize);
                double uIm = -System.Math.Sin(System.Math.PI / butterflySize);

                for (int type = 0; type < numType; type++)
                {
                    for (int j = type; j < dataSize; j += butterflyDistance)
                    {
                        int jp = j + butterflySize;
                        double reTemp = re2[jp] * wRe - im2[jp] * wIm;
                        double imTemp = re2[jp] * wIm + im2[jp] * wRe;
                        re2[jp] = re2[j] - reTemp;
                        im2[jp] = im2[j] - imTemp;
                        re2[j] += reTemp;
                        im2[j] += imTemp;
                    }
                    double tempWRe = wRe * uRe - wIm * uIm;
                    double tempWIm = wRe * uIm + wIm * uRe;
                    wRe = tempWRe;
                    wIm = tempWIm;
                }
            }
        }

        // ビットを左右反転した配列を返す
        private static int[] BitScrollArray(int arraySize)
        {
            int[] reBitArray = new int[arraySize];
            int arraySizeHarf = arraySize >> 1;

            reBitArray[0] = 0;
            for (int i = 1; i < arraySize; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                    reBitArray[j + i] = reBitArray[j] + arraySizeHarf;
                arraySizeHarf >>= 1;
            }
            return reBitArray;
        }
        public static void FFT2D(double[,] inDataRe,double[,] inDataIm,out double[,] outDataRe,out double[,] outDataIm, int xSize, int ySize)
        {
            double[,] tempRe = new double[ySize, xSize];
            double[,] tempIm = new double[ySize, xSize];

            int xbit = GetBitNum(xSize);
            int ybit = GetBitNum(ySize);

            outDataRe = new double[xSize, ySize];
            outDataIm = new double[xSize, ySize];

            for (int j = 0; j < ySize; j++)
            {
                double[] re = new double[xSize];
                double[] im = new double[xSize];
                FFT(
                    GetArray(inDataRe, j),
                    GetArray(inDataIm, j),
                    out re, out im, xbit);

                for (int i = 0; i < xSize; i++)
                {
                    tempRe[j, i] = re[i];
                    tempIm[j, i] = im[i];
                }
            }

            for (int i = 0; i < xSize; i++)
            {
                double[] re = new double[ySize];
                double[] im = new double[ySize];
                FFT(GetArray(tempRe, i), GetArray(tempIm, i), out re, out im, ybit);
                for (int j = 0; j < ySize; j++)
                {
                    outDataRe[i, j] = re[j];
                    outDataIm[i, j] = im[j];
                }
            }
        }
        // ビット数取得
        private static int GetBitNum(int num)
        {
            int bit = -1;
            while (num > 0)
            {
                num >>= 1;
                bit++;
            }
            return bit;
        }

        // 1次元配列取り出し
        private static double[] GetArray(double[,] data2D, int seg)
        {
            double[] reData = new double[data2D.GetLength(0)];
            for (int i = 0; i < data2D.GetLength(0); i++)
            {
                reData[i] = data2D[i, seg];
            }
            return reData;
        }
        // double型をbyte型に変換
        static byte Byte2Int(double num)
        {
            if (num > 255.0) return 255;
            else if (num < 0) return 0;
            else return (byte)num;
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

        static void SaveDouble2Image(double[,] src, string filename)
        {
            Bitmap img = Double2Bitmap(src);
            // 画像の保存
            img.Save(filename);
        }
        // パワースペクトルを計算
        public static double[,] Spectrum(
            double[,] inDataRe,
            double[,] inDataIm,
            int xSize,
            int ySize)
        {
            double[,] data = new double[xSize, ySize];

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    data[i, j] = Math.Log10(
                        Math.Pow(inDataRe[i, j], 2) +
                        Math.Pow(inDataIm[i, j], 2));
                }
            }

            return data;
        }
        /// double2次元配列からbitmapオブジェクトに変換
        /// </summary>
        private static Bitmap Double2Bitmap(double[,] data)
        {
                // サイズ取得
                int xSize = data.GetLength(0);
                int ySize = data.GetLength(1);

                double max = double.MinValue;
                double min = double.MaxValue;
                for (int i = 0; i < xSize; i++)
                {
                    for (int j = 0; j < ySize; j++)
                    {
                        if (max < data[i, j])
                        {
                            max = data[i, j];
                        }
                        if (min > data[i, j])
                        {
                            min = data[i, j];
                        }
                    }
                }

                Bitmap bitmap = new Bitmap(xSize, ySize);
                for (int i = 0; i < xSize; i++)
                {
                    for (int j = 0; j < ySize; j++)
                    {

                        if (max != min)
                        {
                            int cor =
                                (int)((data[i, j] - min) / (max - min) * 255);
                            System.Drawing.Color color =
                                System.Drawing.Color.FromArgb(cor, cor, cor);
                            bitmap.SetPixel(i, j, color);
                        }

                    }
                }

                return bitmap;
        }
    }
}
