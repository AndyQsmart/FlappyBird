using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    private static float maxX = 2.245f;
    private static float maxY = 1.7f;
    private static float minY = -1.1f;
    private static float pertime = 1.2f;

    private float nowIndex = 0;
    private static float deta = 0.1f;
    private static float PI = Mathf.Acos(-1);

    void initRandom()
    {
        nowIndex = 0;
    }

    float getRandom(float from, float to)
    {
        float ans = 0;
        ans = PI * Mathf.Sin(nowIndex) + PI;
        nowIndex = ans + deta;
        if (nowIndex > 2*PI) nowIndex -= 2*PI;
        ans = from + (to - from) * ans / PI / 2;
        return ans;
    }

	IEnumerator PerFrameLoad()
	{
		for (;;)
        {
            if (Game.isRunning())
            {
                Object pipeObj = Resources.Load("Prefabs/Pipe");
                GameObject obj = GameObject.Instantiate(pipeObj) as GameObject;
                //float tmp = Random.Range(minY, maxY);
                float tmp = getRandom(minY, maxY);
                obj.transform.position = new Vector3(maxX, tmp, 4);
            }
            else
            {
                initRandom();
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
