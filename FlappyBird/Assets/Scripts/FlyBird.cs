using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBird : MonoBehaviour
{
    private float maxY = 2.85f;
    private float minY = -2.24f;
    public AudioClip jumpAudio;
    public AudioClip hitAudio;
    private AudioSource audioSource;
    // Use this for initialization
    void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Game.judgeInit())
        {
            this.transform.position = new Vector3(0, 0, 3);
            this.GetComponent<Rigidbody2D>().rotation = 0;
            this.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        if (this.transform.position.y >= maxY || this.transform.position.y <= minY)
        {
            GameOver();
        }
        if (!Game.isRunning())
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryFly();
            }
        }
	}

    public void TryFly()
    {
        Debug.Log("Bird fly");
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
        Game.stopRunning();
        audioSource.clip = hitAudio;
        audioSource.Play();
    }
}
