using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenHouseData : MonoBehaviour {
    public string Color;
    
    public bool IsFriendlyHouse(string skullColor)
    {
        return Color.Equals(skullColor);   
    }
    
    

	// Update is called once per frame
	void Update () {
		
	}
}
