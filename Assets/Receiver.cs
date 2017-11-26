using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class Receiver : MonoBehaviour
{
    public Text inData;
    public Text outData;

    private const int sendPort = 11000;
    private const int receivePort = 11000;
    private const string ourId = "R";

    private UdpClient broadcaster;
    private UdpClient listener;
    private IPEndPoint endPoint;

    private uint counter;

    void Start()
    {
        broadcaster = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Broadcast, sendPort);

        listener = new UdpClient(new IPEndPoint(IPAddress.Any, receivePort));
        listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        counter = 0;

        Application.runInBackground = true;
    }

    void Update()
    {
        if (listener.Available > 0)
        {
            IPEndPoint senderEp = null;
            byte[] data = listener.Receive(ref senderEp);

            string str = Encoding.Unicode.GetString(data);
            if (!str.StartsWith(ourId)) // don't handle our own messages
            {
                inData.text = str;
                Send();
            }
        }
    }

    public void Send()
    {
        string msg = string.Format("{0}:{1:G8}", ourId, ++counter);

        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        broadcaster.Send(buffer, buffer.Length, endPoint);

        outData.text = msg;
    }
}
