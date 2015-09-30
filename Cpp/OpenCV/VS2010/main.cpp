#include <opencv2/opencv.hpp>
#include <opencv2/opencv_lib.hpp>

using namespace cv;

int main(int argc, char *argv[])
{
    Mat im = Mat::zeros(150, 220, CV_8UC3);
    putText(im, "OpenCV TEST", Point(5, 50), FONT_HERSHEY_SIMPLEX, 1, Scalar(0, 0, 200), 2);
    imshow("TEST", im);
    waitKey(0);
    return 0;
}
