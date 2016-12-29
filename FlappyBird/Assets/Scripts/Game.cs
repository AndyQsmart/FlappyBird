using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    private static bool isrunning = false;
    private static bool isInit = true;
    private static int score = 0;
    private static bool is_loged = false;

	// Use this for initialization
	void Start ()
    {
        isrunning = false;
        isInit = true;
        score = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*
        GameObject objects = GameObject.Find("Objects");
        GameObject pipes = objects.transform.Find("Pipes").gameObject;
        pipes.SetActive(false);
        */
    }

    public void tryStart()
    {
        Debug.Log("INFO:Start game.");
        isrunning = true;
        isInit = false;
        score = 0;
        if (!is_loged)
        {
            string username = "";
            username = GameObject.Find("UI Root/StartPage/UserName/InputLabel").GetComponent<UILabel>().text;
            GameObject.Find("UI Root/StartPage/UserName").SetActive(false);
            Network.sendUser(username);
            is_loged = true;
        }
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

    public static void stopRunning()
    {
        isrunning = false;
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

        Network.sendScore(score);

        //bestscore
        int bestscore = 0;
        bestscore = Network.getScore();
        //bestscore = PlayerPrefs.GetInt("BestScore");
        //bestscore = bestscore > score ? bestscore : score;
        PlayerPrefs.SetInt("BestScore", bestscore);
        page.transform.Find("Info/ScoreBoard/BestScore"
            ).gameObject.GetComponent<UILabel>().text = bestscore.ToString();
        Debug.Log("DEBUG: bestscore: " + bestscore);

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
