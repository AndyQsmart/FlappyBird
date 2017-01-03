using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private static float maxX = 3.83f;
    private static float speed = 0.03f;

    // Use this for initialization
    void Start ()
    {
        this.transform.parent = GameObject.Find("Objects/Floors").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Game.isRunning()) return;
        if (this.transform.position.x <= -maxX)
            this.transform.position = new Vector3(maxX, -2.7f, 3);
    }

    void FixedUpdate()
    {
        if (!Game.isRunning()) return;
        this.transform.position += new Vector3(-speed, 0, 0);
        //Debug.Log ("INFO:Pipe move");
    }
}
