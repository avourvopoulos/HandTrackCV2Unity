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

    //[url]https://google.github.io/mediapipe/images/mobile/hand_landmarks.png[/url]
    //IDictionary<string, float> handLandmarks = new Dictionary<string, float>();

    public GameObject  WRIST, INDEX_FINGER_TIP;
    Vector3 p0pos, p8pos;
    public float amp = 1;

    // start
    public void Start()
    {
        init();
    }

    public void Update() {
        WRIST.transform.position = -p0pos;
        INDEX_FINGER_TIP.transform.position = -p8pos;
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
                float [] newdata = ConvertByteToFloat(data);

                //foreach (float coord in newdata){print(coord);}

                //foreach (KeyValuePair <string, float> mark in handLandmarks)
                //  Debug.LogFormat("Key: {0}, Value: {1}", mark.Key, mark.Value);
                // for (int i = 0; i < newdata.Length; i++)
                // {
                //print(newdata[1]+ newdata[2]+ newdata[3]);
                //Instantiate(handPoint, new Vector3(newdata[1], newdata[2], newdata[3]), Quaternion.identity);
                //    Rigidbody clone;
                //    clone = Instantiate(handPoint, transform.position, transform.rotation);
                if (newdata[0] == 0f) {

                    p0pos = new Vector3(newdata[1], newdata[2], newdata[3]) * amp;
                    //print(wristpos);
                    
                }
                if (newdata[0] == 8f)
                {

                    p8pos = new Vector3(newdata[1], newdata[2], newdata[3]) * amp;
                    //print(wristpos);

                }

                //  }
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


    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }


}