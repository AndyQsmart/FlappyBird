using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBird : MonoBehaviour
{
    private static float maxX = 2.245f;
    // Use this for initialization
    void Start ()
    {
        this.transform.parent = GameObject.Find("Objects").transform;
        GameObject obj = GameObject.Find("Objects/FlyBird");
        this.transform.position = new Vector3(
            obj.transform.position.x+0.5f,
            obj.transform.position.y,
            obj.transform.position.z);
        this.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(1.5f, 0, 0);
        this.transform.GetComponent<Rigidbody2D>().angularVelocity = -900;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (Game.judgeInit()) Destroy(this.gameObject);
        //if (!Game.isRunning()) return;

        if (this.transform.position.x > maxX)
        {
            Destroy(this.gameObject);
        }
    }
}
