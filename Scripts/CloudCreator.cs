using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour {
    public List<GameObject> clouds;
    private List<CloudData> activeClouds;
    private int timer = 0;
	// Use this for initialization
	void Start () {
        activeClouds = new List<CloudData>();
        CreateCloud();
	}
	
	// Update is called once per frame
	void Update () {
		if(activeClouds.Count < 6)
        {
            if(timer / activeClouds.Count > 40)
            CreateCloud();
        }

        if(timer > 800)
        {
            timer = 0;
            CleanCloudList();
        }
        timer++;
	}

    void CreateCloud() //generates clouds offscreen.
    {
        float startX = 25, startY = Random.Range(0, 20);
        float xVelocity = Random.Range(.1f, .3f);
        
            startX = -12 + Random.Range(4, 16) ;
       
        GameObject cloud = Instantiate(clouds[0]);
        CloudData data = cloud.GetComponent<CloudData>();
        data.SetPos(new Vector2(startX, startY));
        data.SetVelocity(new Vector2(xVelocity, .001f));

        activeClouds.Add(data);
    }

    void CleanCloudList()
    {
        int a = activeClouds.Count - 1;

        while(a >= 0)
        {
            if(activeClouds[a].GetX() >= 20)
            {

                Destroy(activeClouds[a].gameObject, 3);
                activeClouds.RemoveAt(a);
            }
            a--;
        }
    }
}
