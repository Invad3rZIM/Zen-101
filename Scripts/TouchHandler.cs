
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchHandler : MonoBehaviour
{
    public GameObject cam;
    Vector3 touchPosWorld;
    List<CharacterData> ch;
    GameData data;

    void Start()
    {
        ch = new List<CharacterData>();
        
        if(data == null)
            data = GameObject.Find("GameData").GetComponent<GameData>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            data.ToggleLevelSelect();
        }
        OnComputer();
        return;
        //We check if we have more than one touch happening.
        //We also check if the first touches phase is Ended (that the finger was lifted)
        float units = cam.GetComponent<Camera>().orthographicSize;

        for (int a = 0; a < Input.touchCount; a++)
        {
            Touch t = Input.GetTouch(a);
            
            touchPosWorld = Camera.main.ScreenToWorldPoint(t.position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            
            if (t.phase == TouchPhase.Began)
            {
                int id = t.fingerId;
                
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                int x = (int)(touchPosWorld2D.x );
                int y = (int)(touchPosWorld2D.y );

                if (hitInformation.collider != null)
                {
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    CharacterData data = touchedObject.GetComponent<CharacterData>();
                    
                    if (!data.isTouched())
                    {
                        if(data.Draggable)//updates relevant for tracking moves for stars
                            if (this.data.GetCurrentCritter() == 0 || data.GetID() != this.data.GetCurrentCritter())
                                this.data.NewCritter(data.GetID());

                        data.SetTouch(id); //if the dude isn't being touched, we add it to the list of dudes who are
                        ch.Add(data);
                    }
                }
            }
            else if (Input.GetTouch(a).phase == TouchPhase.Moved)
            {
                int id = t.fingerId;
                
                for (int b = 0; b < ch.Count; b++)
                {
                    if(ch[b].GetTouch() == id)
                    {
                        touchPosWorld = Camera.main.ScreenToWorldPoint(t.position);
                        touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                        ch[b].SetPosition(touchPosWorld2D);
                    }
                }
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            { //Removes the critter from the List of touches when his touch ends.
                int id = t.fingerId;

                for (int b = 0; b < ch.Count; b++)
                {
                    if (ch[b].GetTouch() == id)
                    {
                        ch[b].clearTouch();
                        ch.RemoveAt(b);
                        break;
                    }
                }
            }
        }
    }

    public void OnComputer()
    {
        //We check if we have more than one touch happening.
        //We also check if the first touches phase is Ended (that the finger was lifted)
        float units = cam.GetComponent<Camera>().orthographicSize;


        touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        if (Input.GetMouseButtonDown(0))
        {
            int id = 0;
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            int x = (int)(touchPosWorld2D.x);
            int y = (int)(touchPosWorld2D.y);

            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;
                CharacterData data = touchedObject.GetComponent<CharacterData>();

                if (!data.isTouched())
                {
                    if (data.Draggable) //updates relevant for tracking moves for stars
                        if (this.data.GetCurrentCritter() == 0 || data.GetID() != this.data.GetCurrentCritter())
                        this.data.NewCritter(data.GetID());

                    data.SetTouch(id);
                    ch.Add(data);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            int id = 0;

            for (int b = 0; b < ch.Count; b++)
            {
                if (ch[b].GetTouch() == id)
                {
                    ch[b].clearTouch();
                    ch.RemoveAt(b);
                    break;
                }
            }
        }

        else if (Input.GetMouseButton(0))
        {
            int id = 0;

            for (int b = 0; b < ch.Count; b++)
            {
                if (ch[b].GetTouch() == id)
                {
                    touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                    ch[b].SetPosition(touchPosWorld2D);
                }
            }
        }
    }
}