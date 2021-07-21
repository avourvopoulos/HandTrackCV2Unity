/*
    @author: Thanos
    -----------------------
    UDP-Receive (from python client)
    -----------------------
    // [url]https://google.github.io/mediapipe/solutions/hands[/url]
   
    // > receive
    // 127.0.0.1 : 5005
   
    // send
    // nc -u 127.0.0.1 5005
 
*/
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class ReceiveHandData : MonoBehaviour {

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public string IP = "127.0.0.1"; default local
    public int port; // define > init

    /*
    //[url]https://google.github.io/mediapipe/images/mobile/hand_landmarks.png[/url]
    Dictionary<string, int> handLandmarks = new Dictionary<string, int>(){{"WRIST", 0},
        {"THUMB_CMC", 1}, {"THUMB_MCP", 2}, {"THUMB_IP", 3}, {"THUMB_TIP", 4},
        {"INDEX_FINGER_MCP", 5}, {"INDEX_FINGER_PIP", 6}, {"INDEX_FINGER_DIP", 7}, {"INDEX_FINGER_TIP", 8},
        {"MIDDLE_FINGER_MCP", 9}, {"MIDDLE_FINGER_PIP", 10}, {"MIDDLE_FINGER_DIP", 11}, {"MIDDLE_FINGER_TIP", 12},
        {"RING_FINGER_PIP", 13}, {"RING_FINGER_DIP", 14}, {"RING_FINGER_TIP", 15}, {"PINKY_MCP", 17}, {"PINKY_PIP", 18}, {"PINKY_DIP", 19}, {"PINKY_TIP", 20} };
        */

    public GameObject  WRIST, THUMB_CMC, THUMB_MCP, THUMB_IP, THUMB_TIP, 
        INDEX_FINGER_MCP, INDEX_FINGER_PIP, INDEX_FINGER_DIP, INDEX_FINGER_TIP,
        MIDDLE_FINGER_MCP, MIDDLE_FINGER_PIP, MIDDLE_FINGER_DIP, MIDDLE_FINGER_TIP, RING_FINGER_MCP,
        RING_FINGER_PIP, RING_FINGER_DIP, RING_FINGER_TIP, PINKY_MCP, PINKY_PIP, PINKY_DIP, PINKY_TIP;

    Vector3 p0pos, p1pos, p2pos, p3pos, p4pos, p5pos, p6pos, p7pos, p8pos, p9pos, p10pos, p11pos, p12pos,
         p13pos, p14pos, p15pos, p16pos, p17pos, p18pos, p19pos, p20pos;

    public float gain = 5;

    private int mirror;

    public float[] newdata;

    // start
    public void Start()
    {
        init();

        Mirror = -1;// set -1 for mirroring, else set 1

    //    foreach (KeyValuePair<string, float> mark in handLandmarks)
    //        Debug.LogFormat("Key: {0}, Value: {1}", mark.Key, mark.Value);

    }


    public void Update() {

        WRIST.transform.position = p0pos * Mirror;

        THUMB_CMC.transform.position = p1pos * Mirror;
        THUMB_MCP.transform.position = p2pos * Mirror;
        THUMB_IP.transform.position = p3pos * Mirror;
        THUMB_TIP.transform.position = p4pos * Mirror;

        INDEX_FINGER_MCP.transform.position = p5pos * Mirror;
        INDEX_FINGER_PIP.transform.position = p6pos * Mirror;
        INDEX_FINGER_DIP.transform.position = p7pos * Mirror;
        INDEX_FINGER_TIP.transform.position = p8pos * Mirror;

        MIDDLE_FINGER_MCP.transform.position = p9pos * Mirror;
        MIDDLE_FINGER_PIP.transform.position = p10pos * Mirror;
        MIDDLE_FINGER_DIP.transform.position = p11pos * Mirror;
        MIDDLE_FINGER_TIP.transform.position = p12pos * Mirror;

        RING_FINGER_MCP.transform.position = p13pos * Mirror;
        RING_FINGER_PIP.transform.position = p14pos * Mirror;
        RING_FINGER_DIP.transform.position = p15pos * Mirror;
        RING_FINGER_TIP.transform.position = p16pos * Mirror;

        PINKY_MCP.transform.position = p17pos * Mirror;
        PINKY_PIP.transform.position = p18pos * Mirror;
        PINKY_DIP.transform.position = p19pos * Mirror;
        PINKY_TIP.transform.position = p20pos * Mirror;

    }


    // init
    private void init()
    {
        print("UDPSend.init()");

        // define port
        port = 5005;

        // status
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");

        // Create a receive-data thread
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (true)
        {

            try
            {
                // Receive bytes 
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // convert bytes to float array
                newdata = ConvertByteToFloat(data);

                setPosition(newdata);

            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    public static float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array, i * 4, 4);
            }
            floatArr[i] = BitConverter.ToSingle(array, i * 4);
        }
        return floatArr;
    }

    //mirror tracking
    public int Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    // ToDo: design it better
    public void setPosition(float[] trackdata)
    {
        int bone = (int)trackdata[0];
        switch (bone)
        {
            case 0:
                p0pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 1:
                p1pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 2:
                p2pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 3:
                p3pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 4:
                p4pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 5:
                p5pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 6:
                p6pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 7:
                p7pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 8:
                p8pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 9:
                p9pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 10:
                p10pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 11:
                p11pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 12:
                p12pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 13:
                p13pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 14:
                p14pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 15:
                p15pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 16:
                p16pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 17:
                p17pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 18:
                p18pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 19:
                p19pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;
            case 20:
                p20pos = new Vector3(newdata[1], newdata[2], newdata[3]) * gain;
                break;

        }
    }
    

    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }


}