using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Network : MonoBehaviour
{
    private Socket socket = null;
    private byte[] buffer = new byte[1024];
    private bool is_logged = false;
    private MessageTool message_tool = null;
    private MessageBuffer message_buffer = null;
    private string username = null;
    
	// Use this for initialization
	void Start ()
    {
        IPAddress ip = IPAddress.Parse("192.168.12.47");
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
        socket.ReceiveTimeout = 10;
        socket.Blocking = false;

        is_logged = false;
        message_tool = GameObject.Find("Game").GetComponent<MessageTool>();
        message_buffer = GameObject.Find("Game").GetComponent<MessageBuffer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnDestroy()
    {
        try
        {
            socket.Close();
        }
        catch
        { }
    }

    public void tryLog(string username_str)
    {
        if (is_logged) return;
        username = username_str;
        try
        {
            socket.Send(message_tool.createLogMessage(username));
            is_logged = true;
        }
        catch// (Exception e)
        {

        }
    }

    public bool judgeLog()
    {
        return is_logged;
    }

    public void gameOver(int score)
    {
        try
        {
            socket.Send(message_tool.createOverMessage(username, score));
        }
        catch
        {
        }
    }

    public void sendPosition(float x, float y)
    {
        try
        {
            socket.Send(message_tool.createFlyMessage(username, x, y));
        }
        catch
        { }
    }

    public void getMessage()
    {
        try
        {
            int buffer_len = socket.Receive(buffer);
            message_buffer.addBuffer(buffer, buffer_len);
            //Debug.Log("Debug: buffer len: " + buffer_len);
            //Debug.Log("DEBUG: get message from server");
        }
        catch (System.Exception e)
        {
            //Debug.Log("ERROR: " + e.ToString());
        }
    }

    /*
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
    */
}
