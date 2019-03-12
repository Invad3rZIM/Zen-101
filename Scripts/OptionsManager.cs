using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void StartMenu()
    {
        SceneManager.LoadScene("Start");
    }
	
	public void ShowCredit(int a )
    {
        string person, contact;

        if(a == 0)
        {
            person = "Kirk Zimmer";
            contact = "kzimmer655 @gmail.com";
        }else if(a == 1)
        {
            person = "Sam Spirgel";
            contact = "samspirgel @gmail.com";
        } else
        {
            person = "To Michael";
            contact = "Thanks For All The Guidence, Motivation & Support.";
        }

        GameObject g = GameObject.Find("Canvas").gameObject.transform.FindChild("RawImage").gameObject.transform.FindChild("CreditPage").gameObject;
        GameObject f = GameObject.Find("Canvas").gameObject.transform.FindChild("RawImage").gameObject.transform.FindChild("Title").gameObject;

        g.transform.FindChild("Person").GetComponent<Text>().text = person;
        g.transform.FindChild("Contact").GetComponent<Text>().text = contact;

        g.SetActive(true);
    }

    public void Hide()
    {
        GameObject g = GameObject.Find("Canvas").gameObject.transform.FindChild("RawImage").gameObject.transform.FindChild("CreditPage").gameObject;
        g.SetActive(false);

    }
}
