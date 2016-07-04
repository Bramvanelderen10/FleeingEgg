using UnityEngine;
using System.Collections;

public class backgroundScroll : GameComponent {

    public float speed = 0.5f;

    private float startTime;
    private float time;
    private float adjustment =  0;

	// Use this for initialization
	void Start () {
        isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            if ((Time.time * speed) - adjustment > 1)
            {
                adjustment += 1;
            }
            Vector2 offset = new Vector2((Time.time * speed) - adjustment, 0);
            GetComponent<Renderer>().material.mainTextureOffset = offset;
        }
    }
}
