using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    private static bool isrunning = false;
    private static bool isInit = true;
    private static int score = 0;
    private Network network = null;
    private MessageBuffer message_buffer = null;
    private MessageTool message_tool = null;
    private FlyBird fly_bird = null;

	// Use this for initialization
	void Start ()
    {
        isrunning = false;
        isInit = true;
        score = 0;
        network = GameObject.Find("Game").GetComponent<Network>();
        message_buffer = GameObject.Find("Game").GetComponent<MessageBuffer>();
        message_tool = GameObject.Find("Game").GetComponent<MessageTool>();
        fly_bird = GameObject.Find("Objects/FlyBird").GetComponent<FlyBird>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //send
        /*
        if (isrunning)
        {
            network.sendPosition(fly_bird.getX(), fly_bird.getY());
        }
        */

        if (!message_buffer.isAlmostFull())
            network.getMessage();
        /*
        try
        {
            score = message_buffer.getBufferSize() - 1;
            addScore();
        }
        catch { }
        */

        //get
        for (;;)
        {
            byte[] package = null;
            int buffer_len = 0;
            if (!message_buffer.getPackage(ref package, ref buffer_len)) break;
            int type = message_tool.getMessageType(package);
            Debug.Log("DEBUG: message type: " + type);
            //
            if (type == (int)MessageTool.MessageType.CS_LOGIN)
            {
                string username = "";
                message_tool.getLogMessage(package, ref username);
                Debug.Log("DEBUG: " + username + " log in");
            }
            else if (type == (int)MessageTool.MessageType.CS_POSITION)
            {
                string username = "";
                float x = 0, y = 0;
                message_tool.getFlyMessage(package, ref username, ref x, ref y);
                //Debug.Log("DEBUG: username x: " + username + " " + x + " " + y);
                GameObject other_bird = getOtherBird(username);
                Vector3 position = other_bird.transform.position;
                position.x = x-fly_bird.getX();
                position.y = y;
                other_bird.transform.position = position;
            }
            else if (type == (int)MessageTool.MessageType.CS_OVER)
            {
                //
                string username = "";
                int score = 0;
                message_tool.getOverMessage(package, ref username, ref score);
                Debug.Log("INFO: get score " + score);
                try
                {
                    GameObject objects = GameObject.Find("Objects/" + username);
                    objects.SetActive(false);
                }
                catch
                { }
            }
        }
    }

    void FixedUpdate()
    {
        if (isrunning)
        {
            network.sendPosition(fly_bird.getX(), fly_bird.getY());
        }
    }

    GameObject getOtherBird(string username)
    {
        GameObject other_bird = GameObject.Find("Objects/" + username);
        if (other_bird == null)
        {
            GameObject objects = GameObject.Find("Objects");
            GameObject birdObj = Resources.Load("Prefabs/OtherBird") as GameObject;
            other_bird = GameObject.Instantiate(birdObj) as GameObject;
            other_bird.name = username;
            other_bird.transform.parent = objects.transform;
        }
        return other_bird;
    }

    public void tryStart()
    {
        Debug.Log("INFO:Start game.");
        //init
        isrunning = true;
        isInit = false;
        score = 0;

        //try log
        string username = "";
        username = GameObject.Find("UI Root/StartPage/UserName/InputLabel").GetComponent<UILabel>().text;
        GameObject.Find("UI Root/StartPage/UserName").SetActive(false);
        network.tryLog(username);

        //page change
        GameObject.Find("UI Root/StartPage").SetActive(false);
        GameObject objects = GameObject.Find("UI Root");
        objects.transform.Find("ScoreLabel").gameObject.SetActive(true);
        GameObject.Find("UI Root/ScoreLabel").GetComponent<UILabel>().text = score.ToString();
        /*
        GameObject objects = GameObject.Find("Objects");
        GameObject pipes = objects.transform.Find("Pipes").gameObject;
        pipes.SetActive(true);
        */
    }

    public void tryReset()
    {
        Debug.Log("INFO:Reset game.");
        isrunning = false;
        isInit = true;
        score = 0;
        GameObject.Find("UI Root/EndPage").SetActive(false);
        GameObject objects = GameObject.Find("UI Root");
        GameObject page = objects.transform.Find("StartPage").gameObject;
        page.SetActive(true);
    }

    public static bool isRunning()
    {
        return isrunning;
    }

    public void stopRunning()
    {
        //init
        isrunning = false;
        network.gameOver(score);

        //page change
        GameObject objects = GameObject.Find("UI Root");
        GameObject page = objects.transform.Find("EndPage").gameObject;
        page.SetActive(true);
        try
        {
            GameObject.Find("UI Root/ScoreLabel").SetActive(false);
        }
        catch (Exception e)
        {
        }

        //bestscore
        int bestscore = 0;
        bestscore = PlayerPrefs.GetInt("BestScore");
        bestscore = bestscore > score ? bestscore : score;
        PlayerPrefs.SetInt("BestScore", bestscore);
        page.transform.Find("Info/ScoreBoard/BestScore"
            ).gameObject.GetComponent<UILabel>().text = bestscore.ToString();
        //Debug.Log("DEBUG: bestscore: " + bestscore);

        //score
        GameObject scores = page.transform.Find("Info/ScoreBoard/Score").gameObject;
        scores.GetComponent<UILabel>().text = score.ToString();
        //Debug.Log("INFO:Score: " + score);
        score = 0;
    }

    public static bool judgeInit()
    {
        return isInit;
    }

    public static void addScore()
    {
        score++;
        GameObject.Find("UI Root/ScoreLabel").GetComponent<UILabel>().text = score.ToString();
    }

    public static int getScore()
    {
        return score;
    }
}
