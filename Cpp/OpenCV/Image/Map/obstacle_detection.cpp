#include "opencv2\opencv.hpp"
using namespace cv;

int main(int argc, char* argv[])
{
    Mat im, hsv, mask;					// 画像オブジェクトの宣言
    cvtColor(im, hsv, CV_BGR2HSV);      // 画像をRGBからHSVに変換
    inRange(hsv, Scalar(150, 70, 70), Scalar(360, 255, 255), mask);	// 色検出でマスク画像の作成
    imshow("Mask", mask);               // マスク画像の作成
    waitKey(0);
    return 0;
}
