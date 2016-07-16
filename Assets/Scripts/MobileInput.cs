using UnityEngine;
using System.Collections;

public class MobileInput : MonoBehaviour {

    public static MobileInput Instance;

    public bool x = false;
    public bool y = false;
    public bool a = false;
    public bool b = false;


	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ButtonDown(string type)
    {
        switch (type)
        {
            case "x":
                x = true;                
                break;
            case "y":
                y = true;
                break;
            case "a":
                a = true;
                break;
            case "b":
                b = true;
                break;
        }

        StartCoroutine(DisableButton(type));
    }

    public bool GetButtonDown(string type) 
    {
        bool result = false;
        switch (type)
        {
            case "X":
                result = x;
                break;
            case "Y":
                result = y;
                break;
            case "A":
                result = a;
                break;
            case "B":
                result = b;
                break;
        }

        return result;
    }

    public void Reset()
    {
        a = false;
        b = false;
        x = false;
        y = false;
    }

    IEnumerator DisableButton(string type)
    {
        yield return new WaitForFixedUpdate();
        
        switch (type)
        {
            case "x":
                x = false;
                break;
            case "y":
                y = false;
                break;
            case "a":
                a = false;
                break;
            case "b":
                b = false;
                break;
        }
    }
}
