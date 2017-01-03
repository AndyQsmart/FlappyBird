using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBird : MonoBehaviour
{
    private static float speed = 0.03f;

    Game game = null;
    
    public AudioClip jumpAudio;
    public AudioClip hitAudio;
    private AudioSource audioSource;

    private float nowx;
    private float nowy;

    // Use this for initialization
    void Start ()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        audioSource = this.GetComponent<AudioSource>();
        nowx = nowy = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Game.judgeInit())
        {
            this.transform.position = new Vector3(0, 0, 3);
            this.GetComponent<Rigidbody2D>().rotation = 0;
            this.GetComponent<Rigidbody2D>().angularVelocity = 0;
            nowx = nowy = 0;
        }
        if (!Game.isRunning())
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            nowx = nowy = 0;
        }
        else
        {
            nowy = this.transform.position.y;
            this.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                TryFly();
            }
        }
	}

    void FixedUpdate()
    {
        if (!Game.isRunning()) return;
        nowx += speed;
    }

    public float getX()
    {
        return nowx;
    }

    public float getY()
    {
        return nowy;
    }

    public void TryFly()
    {
        //Debug.Log("Bird fly");
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 2.5f, 0);
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameOver();
    }

    void GameOver()
    {
        if (!Game.isRunning()) return;
        Debug.Log("INFO:Game over");
        game.stopRunning();
        audioSource.clip = hitAudio;
        audioSource.Play();
    }
}
