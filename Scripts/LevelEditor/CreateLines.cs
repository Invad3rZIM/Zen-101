using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLines : MonoBehaviour {
    public GameObject line;
    public int width, height;
	// Use this for initialization
	void Start () {
		for(int a =-1; a < width+1; a++)
        {
            GameObject l = Instantiate(line);
            LineRenderer rend = l.GetComponent<LineRenderer>();
            rend.numPositions = 2;
            rend.SetPosition(0, new Vector3( a, 0));
            rend.SetPosition(1, new Vector3( a, height ));
        }


        for (int a = -1; a < height+1; a++)
        {
            GameObject l = Instantiate(line);
            LineRenderer rend = l.GetComponent<LineRenderer>();
            rend.numPositions = 2;
            rend.SetPosition(0, new Vector3( 0, a,0));
            rend.SetPosition(1, new Vector3( width , a, 0));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
