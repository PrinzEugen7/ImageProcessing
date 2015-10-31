# -*- coding: utf-8 -*-
import numpy as np
import matplotlib.pyplot as plt
from PIL import Image

def main():
    im = Image.open("test.jpg") # 画像の取得
    im = np.array(im)           # 画像データをNumPyリストに変換
    plt.imshow(im)              # 画像貼り付け
    plt.show()                  # グラフ表示

if __name__ == '__main__':
    main()
