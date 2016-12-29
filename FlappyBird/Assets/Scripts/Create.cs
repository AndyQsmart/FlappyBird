using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    private static float maxX = 2.245f;
    private static float maxY = 1.7f;
    private static float minY = -1.1f;
    private static float pertime = 1.2f;
	IEnumerator PerFrameLoad()
	{
		for (;;)
        {
            if (Game.isRunning())
            {
                Object pipeObj = Resources.Load("Prefabs/Pipe");
                GameObject obj = GameObject.Instantiate(pipeObj) as GameObject;
                Object planeObj = Resources.Load("Prefabs/plane");
                GameObject obj2 = GameObject.Instantiate(planeObj) as GameObject;
                float tmp = Random.Range(minY, maxY);
                obj.transform.position = new Vector3(maxX, tmp, 4);
                obj2.transform.position = new Vector3(maxX, tmp, 4);
            }
			yield return new WaitForSeconds (pertime);
		}
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(PerFrameLoad ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
