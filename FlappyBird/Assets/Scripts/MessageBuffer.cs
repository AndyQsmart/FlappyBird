using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class MessageBuffer : MonoBehaviour
{
    static int MAXSIZE = 10240;
    private byte[] buffer = null;
    private int from, to;

	// Use this for initialization
	void Start ()
    {
        from = to = 0;
        buffer = new byte[MAXSIZE];
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void addBuffer(byte[] new_buffer, int buffer_len)
    {
        if ((to - from + MAXSIZE) % MAXSIZE + buffer_len >= MAXSIZE) return;
        //Debug.Log("######" + buffer_len);
        for (int i = 0; i < buffer_len; ++i)
        {
            buffer[to] = new_buffer[i];
            to = (to + 1) % MAXSIZE;
        }
        //Debug.Log("DEBUG: from: " + from + " to: "+to);
    }

    public bool getPackage(ref byte[] package, ref int buffer_len)
    {
        //Debug.Log("####### " + from +" "+ to);
        if ((to - from + MAXSIZE) % MAXSIZE < 4) return false;
        //get package len (head)
        int len = 0;
        List<byte> byte_list = new List<byte>();
        for (int i = 0; i < 4; ++i)
            byte_list.Add(buffer[(from+i)%MAXSIZE]);
        len = System.BitConverter.ToInt32(byte_list.ToArray(), 0);
        len = IPAddress.NetworkToHostOrder(len);
        //Debug.Log("******Message len " + len);

        if ((to - from + MAXSIZE) % MAXSIZE < 4 + len) return false;

        //deal package??
        buffer_len = len + 4;
        package = new byte[buffer_len];
        for (int i = 0; i < buffer_len; ++i)
            package[i] = buffer[(from + i) % MAXSIZE];
        from = (from + buffer_len) % MAXSIZE;
        return true;
    }

    public bool isAlmostFull()
    {
        if ((to - from + MAXSIZE) % MAXSIZE < MAXSIZE - 1024) return false;
        else return true;
    }

    public int getBufferSize()
    {
        return (to - from + MAXSIZE) % MAXSIZE;
    }
}
