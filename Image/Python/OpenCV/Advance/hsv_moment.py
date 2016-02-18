# -*- coding: utf-8 -*-
import cv2
import numpy as np

def main():
    im = cv2.imread("test.jpg") # 画像の取得
    hsv = cv2.cvtColor(im, cv2.COLOR_BGR2HSV)   # HSV変換
    # 青色のHSV範囲
    hsv_min = np.array([80, 150, 0])
    hsv_max = np.array([150, 255, 255])
    # 緑色のHSV範囲
    #hsv_min = np.array([10, 80, 0])
    #hsv_max = np.array([80, 255, 255])
    mask = cv2.inRange(hsv, hsv_min,  hsv_max)
    m = cv2.countNonZero(mask)
    h, w = mask.shape
    per = round(100*float(m)/(w * h),1)
    print("Moment[px]:",m)
    print("Percent[%]:", per)
    cv2.imshow("Mask",mask)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

if __name__ == '__main__':
    main()
