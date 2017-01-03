using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	private static float maxX = 2.245f;
	private static float speed = 0.03f;
    private bool isPassed = false;

	// Use this for initialization
	void Start ()
    {
        this.transform.parent = GameObject.Find("Objects/Pipes").transform;
        this.isPassed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Game.judgeInit()) Destroy(this.gameObject);
        if (!Game.isRunning()) return;

		if (this.transform.position.x < -maxX)
        {
			Destroy (this.gameObject);
		}

        if (this.transform.position.x < 0 && !isPassed)
        {
            Game.addScore();
            isPassed = true;
            //Debug.Log("INFO:Score: "+Game.getScore());
        }
	}

    void FixedUpdate()
    {
        if (!Game.isRunning()) return;
        this.transform.position += new Vector3(-speed, 0, 0);
    }
}
