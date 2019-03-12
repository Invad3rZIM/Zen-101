using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBottom : MonoBehaviour {
    private RectTransform rect;
	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();

        if (Camera.main.aspect <= .58)
        {
            Debug.Log("16:9");
        }
        else
        {
            Debug.Log(rect.localPosition);
            Debug.Log(rect.position);
            rect.position = new Vector3(rect.position.x, 95, rect.position.z);
            Debug.Log("16: 10");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
