# Adapted from: https://github.com/Ankit0822/Hand-Tracking-using-opencv-python-and-mediapipe
# Prereqs:
# pip install opencv-python
# pip install mediapipe

import cv2
import mediapipe as mp
import time
import socket
import numpy as np
from struct import *

UDP_IP = "127.0.0.1"
UDP_PORT = 5005

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP

# Create Video object
cap = cv2.VideoCapture(0)
# Formality we have to write before start
# using this model
mpHands = mp.solutions.hands
# Creating an object from class Hands
hands = mpHands.Hands()
# creating an object to draw hand landmarks
mpDraw = mp.solutions.drawing_utils
# Previous time for frame rate
pTime = 0
# Current time for frame rate
cTime = 0

while True:
    # Getting our Frame
    success, img = cap.read()
    # Convert image into RGB
    imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    # Calling the hands object to the getting results
    results = hands.process(imgRGB)
    #print(results.multi_hand_landmarks)

    # Checking something is detected or not
    if results.multi_hand_landmarks :
        # Extracting the multiple hands
        # Go through each hand
        for handLms in results.multi_hand_landmarks:
            # Getting id(index number) and landmark of each hand
            for id, lm in enumerate(handLms.landmark):
                print(id,lm)
                #print(type(id))
                #bytes object containing the values packed according to the format
                coord_struct = pack('!ffff', float(id), lm.x, lm.y, lm.z)
                #print(coord_struct)
                #send via UDP
                sock.sendto(coord_struct, (UDP_IP, UDP_PORT))
                # Height, width and channel of image
                h, w, c = img.shape
                # X and Y coordinate
                # their values in decimal so
                # we have to convert into pixel
                cx, cy = int(lm.x*w),int(lm.y*h)
                #print(id, cx, cy)
                #if id ==4:
                cv2.circle(img, (cx, cy), 7, (255,0,255), cv2.FILLED)
            # Draw the landmarks and line of the each hands
            mpDraw.draw_landmarks(img, handLms, mpHands.HAND_CONNECTIONS)
    # Getting the current time
    cTime = time.time()
    # Getting frame per second
    fps = 1/(cTime-pTime)
    # Previous time become current time
    pTime = cTime
    # Labeling the Frame rate
    cv2.putText(img,str(int(fps)),(10,70),cv2.FONT_HERSHEY_PLAIN,3,
                            (255,0,255),3)

    cv2.imshow("image",img) # Show the frame
    cv2.waitKey(1) # Wait for 1 millisecond
