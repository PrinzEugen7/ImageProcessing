# -*- coding: utf-8 -*-
import cv2
import matplotlib.pyplot as plt

def main():
    im = cv2.imread("test.jpg") # 画像の取得
    im = cv2.cvtColor(im, cv2.COLOR_BGR2RGB)
    plt.imshow(im)              # 画像貼り付け
    plt.show()                  # グラフ表示

if __name__ == '__main__':
    main()
