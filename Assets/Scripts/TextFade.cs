using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFade : MonoBehaviour {

    private bool isFadingIn = false;
    private float time;
    private Text text;

	// Use this for initialization
	void Start ()
    {
        time = Time.time;
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Color color = text.color;
        if (isFadingIn)
        {            
            color.a = Time.time - time;
            text.color = color;
            if (color.a >= 1)
            {
                isFadingIn = false;
                time = Time.time;
            }
        } else
        {
            color.a = 1 - (Time.time - time);
            text.color = color;
            if (color.a <= 0)
            {
                isFadingIn = true;
                time = Time.time;
            }
        }	
	}
}
