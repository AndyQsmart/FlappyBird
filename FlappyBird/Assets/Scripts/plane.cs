using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    private static float maxX = 2.245f;
    private static float maxY = 1.7f;
    private static float minY = -1.1f;
    private static float speed = 0.03f;

    // Use this for initialization
    void Start ()
    {
        this.transform.parent = GameObject.Find("Objects/Pipes").transform;
        this.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(-3, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Game.judgeInit()) Destroy(this.gameObject);
        if (!Game.isRunning()) return;

        if (this.transform.position.x < -maxX)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        //if (!Game.isRunning()) return;
        //this.transform.position += new Vector3(-speed, 0, 0);
    }
}
