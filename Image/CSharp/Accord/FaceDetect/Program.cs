using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 画像の取得
            var img = new Bitmap(@"src.jpg");
            // グレースケール化
            //var gray = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721).Apply(img);
            // カスケード識別器の読み込み
            var cascadeFace = Accord.Vision.Detection.Cascades.FaceHaarCascade.FromXml(@"haarcascade_frontalface_default.xml");
            // Haar-Like特徴量による物体検出を行うクラスの生成
            var detectorFace = new Accord.Vision.Detection.HaarObjectDetector(cascadeFace);

            // 読み込んだ画像から顔の位置を検出（顔の位置はRectangle[]で返される）
            var faces = detectorFace.ProcessFrame(img);

            // 画像に検出された顔の位置を書き込みPictureBoxに表示
            var markerFaces = new Accord.Imaging.Filters.RectanglesMarker(faces, Color.Yellow);
            img = markerFaces.Apply(img);
            // 保存
            //Bitmap img2 = markerFaces.ToBitmap();
            img.Save(@"dst.jpg");
            img.Dispose();
        }
    }
}
