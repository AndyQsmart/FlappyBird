using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Network : MonoBehaviour
{
    private static Socket socket = null;
    private static byte[] buffer = new byte[1024];
	// Use this for initialization
	void Start ()
    {
        IPAddress ip = IPAddress.Parse("192.168.180.128");
        socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
        try
        {
            socket.Connect(new IPEndPoint(ip, 8888));
            Debug.Log("INFO:connet to server");
        }
        catch
        {
            Debug.Log("ERROR:failed to connect server");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void sendUser(string username)
    {
        try
        {
            socket.Send(Encoding.ASCII.GetBytes(username));
        }
        catch
        {
            Debug.Log("ERROR: Send uername error");
        }
    }

    public static void sendScore(int score)
    {
        try
        {
            Debug.Log("INFO:Send score :" + score.ToString());
            socket.Send(Encoding.ASCII.GetBytes(score.ToString()));
        }
        catch
        {
            Debug.Log("ERROR: Send score error");
        }
    }

    public static int getScore()
    {
        try
        {
            int buffer_len = socket.Receive(buffer);
            int score = int.Parse(Encoding.ASCII.GetString(buffer, 0, buffer_len));
            return score;
        }
        catch
        {
            return 0;
        }
    }
}
