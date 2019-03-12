using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButtons : MonoBehaviour {
    public int value, state;
    private GameData data;
    private Button button;
    private Image image;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        SetNumber();
        ConfigureState();
    }
	
    void SetNumber()
    {
        value = value + (data.GetLevelGrouping() * 15);

        if (value > 101 || value < 1)
            gameObject.SetActive(false);

        transform.FindChild("Number").gameObject.GetComponent<Text>().text = "" + value;
    }

    void ConfigureState()
    {
        state = data.GetLevelState(value);

        if(state == 0)
        {
            image.color = new UnityEngine.Color(.624f, .471f, .471f);
        }
        if (state == 2)
        {
           // image.color = new UnityEngine.Color(.58f, .28f, 0f);
            transform.FindChild("StarManager").gameObject.transform.FindChild("Bronze").gameObject.SetActive(true);

            transform.FindChild("StarManager").gameObject.transform.FindChild("Bronze").gameObject.transform.FindChild("Star (1)").gameObject.SetActive(true);
        }
        if (state == 3)
        {
         //   image.color = new UnityEngine.Color(.361f, .361f, .361f);

            transform.FindChild("StarManager").gameObject.transform.FindChild("Silver").gameObject.SetActive(true);
        }
        if (state == 4)//EBFF35FF
        {
         //   image.color = new UnityEngine.Color(1f, .71f, .102f);
            UnityEngine.Color c = new UnityEngine.Color(.92f, 1f, .207f);

            GameObject g = transform.FindChild("StarManager").gameObject.transform.FindChild("Gold").gameObject;
            g.SetActive(true);
            g.transform.FindChild("Star (1)").gameObject.GetComponent<Image>().color = c;
            g.transform.FindChild("Star (2)").gameObject.GetComponent<Image>().color = c;
            g.transform.FindChild("Star (3)").gameObject.GetComponent<Image>().color = c;
        }

    }
    // Update is called once per frame
    void Update () {
		
	}
}
