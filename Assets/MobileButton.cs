using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using QuickTimeEvent;

public class MobileButton : MonoBehaviour {

    public Type type;
    private PlayerController pc;

	// Use this for initialization
	void Start () {
        FindPlayer();
    }
	
	// Update is called once per frame
	void Update () {
	    if (!pc)
        {
            FindPlayer();
        }
	}

    void FindPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            transform.GetComponent<Button>().onClick.AddListener(delegate { pc.OnClick(type); });
        }
    }
}
