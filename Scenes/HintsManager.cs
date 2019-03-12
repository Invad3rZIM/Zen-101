using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsManager : MonoBehaviour {
    private GameData data;
    
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        
        transform.FindChild("Hint1").gameObject.SetActive(true);

        if (data.TotalHints() > 1)
            transform.FindChild("Hint2").gameObject.SetActive(true);
        if(data.TotalHints() > 2)
            transform.FindChild("Hint3").gameObject.SetActive(true);

    }
}
