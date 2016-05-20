using UnityEngine;
using System.Collections;

public class backgroundScroll : GameComponent {

    public float speed = 0.5f;

	// Use this for initialization
	void Start () {
        isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            Vector2 offset = new Vector2(Time.time * speed, 0);
            GetComponent<Renderer>().material.mainTextureOffset = offset;
        }
    }
}
