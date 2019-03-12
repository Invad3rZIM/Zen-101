using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActualHint : MonoBehaviour {
    private GameData data;
    private Button button;
    private Image image;
    private Text text;
    public int buttonNumber;
    private bool hintShowing = false;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        button = GetComponent<Button>();
        image = transform.FindChild("Image").gameObject.GetComponent<Image>();
        text = transform.FindChild("Text").gameObject.GetComponent<Text>();

        text.text = data.GetHint(buttonNumber - 1);

        ShowHint();
    }

    private void ShowHint()
    {
        if (data.HintsUnlocked() >= buttonNumber)
        {
            image.enabled = false;
            button.enabled = false;
            text.enabled = true;
            text.gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(!hintShowing)
            ShowHint();
	}
}
