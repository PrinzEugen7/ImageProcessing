# -*- coding:utf-8 -*-
import cv2

def main():
    cap = cv2.VideoCapture("test.avi")

    while(cap.isOpened()):
        ret, frame = cap.read()
        cv2.imshow('frame',frame)
        key = cv2.waitKey(1)

        # Escキー押したら終了
        if key == 27:
            break

    cap.release()
    cv2.destroyAllWindows()

if __name__ == "__main__":
    main()
