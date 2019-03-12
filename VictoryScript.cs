using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THis Script is called as a consequence to beating the level. It loads the "YOU WIN CHAMP"
//screen.

public class VictoryScript : MonoBehaviour {
    GameData data;
    RawImage image;
    Text message;
    Image retry;
    Image next;
    private float alpha = -.5f;
    private int state;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        image = GetComponent<RawImage>();
        message = transform.FindChild("Message").gameObject.GetComponent<Text>();
        retry = transform.FindChild("ButtonHolder").gameObject.transform.FindChild("Retry").GetComponent<Image>();
        next = transform.FindChild("ButtonHolder").gameObject.transform.FindChild("Continue").GetComponent<Image>();

        SetWinColor();
        CalcState();
    }

    //Is The Screen Bronze, Silver, Gold?
    void SetWinColor()
    {
        int a = data.WinQuality;
        if (a == 2)//877A9CFF
            image.color = new UnityEngine.Color(.52f, .476f, .609f, 0);
        if (a == 1)//C57107FF
            image.color = new UnityEngine.Color(.969f, .641f, .227f, 0);

    }

    void CalcState()
    {
        state = GameObject.Find("Canvas").gameObject.transform.FindChild("RawImage").gameObject.transform.FindChild("StarManager").gameObject.GetComponent<StarManager>().state;
        data.UpdateState(data.currentLevel, state);
        data.UpdateState(data.currentLevel + 1, 1);
    }


    // Update is called once per frame
    void Update () {
        if (alpha < 0)
            alpha += .02f;
        if (alpha > 0 && alpha < 1)
        {
            alpha += .02f;
            image.color = new UnityEngine.Color(image.color.r, image.color.g, image.color.b, alpha);
            message.color = new UnityEngine.Color(message.color.r, message.color.g, message.color.b, alpha);

            retry.color = new UnityEngine.Color(retry.color.r, retry.color.g, retry.color.b, alpha);
            next.color = new UnityEngine.Color(next.color.r, next.color.g, next.color.b, alpha);
        }

        
	}
}
