using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QuickTimeEvent;

public class MultiplePressChecker : MonoBehaviour {
    public PlayerController pc;
    public List<Button> buttons;

    bool x = false;
    bool y = false;
    bool a = false;
    bool b = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckMultiplePresses())
        {
            pc.AddMiss();
        }
	}

    bool CheckMultiplePresses()
    {
        bool result = false;
        int count = 0;
        if (x)
        {
            count++;
        }
        if (y)
        {
            count++;
        }
        if (a)
        {
            count++;
        }
        if (b)
        {
            count++;
        }

        if (count > 1)
            result = true;

        return result;
    }

    public void ButtonUp(string type)
    {
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
    }
}
