using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudData : MonoBehaviour {
    private float x, y, vX, vY;

	// Use this for initialization
	void Start () {
        x = transform.position.x;
        y = transform.position.y;

        vX = .01f;
	}

    public void SetPos(Vector2 p)
    {
        this.x = p.x;
        this.y = p.y;

        transform.position = new Vector3(this.x, this.y, 0);
    }

    public void SetVelocity(Vector2 v)
    {
        this.vX = v.x;
        this.vY = v.y;
    }

    public float GetX()
    {
        return this.x;
    }
	
	// Update is called once per frame
	void Update () {
        this.x += this.vX;
        this.y += this.vY;
        transform.position = new Vector3(this.x, this.y, 0);
	}
}
