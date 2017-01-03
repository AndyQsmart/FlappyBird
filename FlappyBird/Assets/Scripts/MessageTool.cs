using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MessageTool : MonoBehaviour
{
    public enum MessageType
    {
        CS_LOGIN = 0,
        CS_POSITION = 1,
        CS_OVER = 2,
        SC_LOGIN = 3,
        SC_POSITION = 4,
        SC_OVER = 5
    };

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //添加包头，包长度
    private byte[] addHead(byte[] buffer)
    {
        byte[] package = null;
        int buffer_len = IPAddress.HostToNetworkOrder((int)buffer.Length);
        List<byte> byte_list = new List<byte>();
        byte_list.AddRange(System.BitConverter.GetBytes(buffer_len));
        byte_list.AddRange(buffer);
        package = byte_list.ToArray();
        return package;
    }

    public int getMessageType(byte[] package)
    {
        int type = System.BitConverter.ToInt32(package, 4);
        type = IPAddress.NetworkToHostOrder(type);
        //Debug.Log("INFO: message type: " + type);
        return type;
    }

    //create a log message
    public byte[] createLogMessage(string username)
    {
        List<byte> byte_list = new List<byte>();
        byte[] message = null;

        //add type
        int type = IPAddress.HostToNetworkOrder((int)MessageType.CS_LOGIN);
        byte_list.AddRange(System.BitConverter.GetBytes(type));

        //add string length
        int username_len = IPAddress.HostToNetworkOrder((int)username.Length);
        byte_list.AddRange(System.BitConverter.GetBytes(username_len));

        //add string
        message = Encoding.ASCII.GetBytes(username);
        byte_list.AddRange(message);

        message = byte_list.ToArray();
        return addHead(message);
    }

    public void getLogMessage(byte[] package, ref string username)
    {
        int username_len = System.BitConverter.ToInt32(package, 8);
        username_len = IPAddress.NetworkToHostOrder(username_len);
        List<byte> byte_list = new List<byte>();
        int len = username_len + 12;
        for (int i = 12; i < len; ++i)
            byte_list.Add(package[i]);
        username = System.Text.Encoding.ASCII.GetString(byte_list.ToArray());
    }

    public byte[] createFlyMessage(string username, float x, float y)
    {
        List<byte> byte_list = new List<byte>();
        byte[] message = null;

        //add type
        int type = IPAddress.HostToNetworkOrder((int)MessageType.CS_POSITION);
        byte_list.AddRange(System.BitConverter.GetBytes(type));

        //add string length
        int username_len = IPAddress.HostToNetworkOrder((int)username.Length);
        byte_list.AddRange(System.BitConverter.GetBytes(username_len));

        //add string
        message = Encoding.ASCII.GetBytes(username);
        byte_list.AddRange(message);

        //add x
        byte_list.AddRange(System.BitConverter.GetBytes(x));

        //add y
        byte_list.AddRange(System.BitConverter.GetBytes(y));

        message = byte_list.ToArray();
        return addHead(message);
    }

    public void getFlyMessage(byte[] package, ref string username, ref float x, ref float y)
    {
        int username_len = System.BitConverter.ToInt32(package, 8);
        username_len = IPAddress.NetworkToHostOrder(username_len);
        List<byte> byte_list = new List<byte>();
        int len = username_len + 12;
        for (int i = 12; i < len; ++i)
            byte_list.Add(package[i]);
        username = System.Text.Encoding.ASCII.GetString(byte_list.ToArray());

        x = System.BitConverter.ToSingle(package, len);
        y = System.BitConverter.ToSingle(package, len + 4);
    }

    public byte[] createOverMessage(string username, int score)
    {
        List<byte> byte_list = new List<byte>();
        byte[] message = null;

        //add type
        int type = IPAddress.HostToNetworkOrder((int)MessageType.CS_OVER);
        byte_list.AddRange(System.BitConverter.GetBytes(type));

        //add string length
        int username_len = IPAddress.HostToNetworkOrder((int)username.Length);
        byte_list.AddRange(System.BitConverter.GetBytes(username_len));

        //add string
        byte_list.AddRange(Encoding.ASCII.GetBytes(username));

        //add score
        byte_list.AddRange(System.BitConverter.GetBytes(score));

        message = byte_list.ToArray();
        return addHead(message);
    }

    public void getOverMessage(byte[] package, ref string username, ref int score)
    {
        int username_len = System.BitConverter.ToInt32(package, 8);
        username_len = IPAddress.NetworkToHostOrder(username_len);
        List<byte> byte_list = new List<byte>();
        int len = username_len + 12;
        for (int i = 12; i < len; ++i)
            byte_list.Add(package[i]);
        username = System.Text.Encoding.ASCII.GetString(byte_list.ToArray());

        score = System.BitConverter.ToInt32(package, len);
    }
}
