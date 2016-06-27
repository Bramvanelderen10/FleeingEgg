using UnityEngine;
using System.Collections;

public class HandAI : MonoBehaviour {

    private GameObject player;
    private Animator anim;
    private float posX;
    private bool hold = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        if (!anim)
            anim = GetComponent<Animator>();
        if (player && anim && !hold && player.GetComponent<PlayerController>().isActive) {
            if (Vector2.Distance(transform.position, player.transform.position) < 3.5f) //TODO CHECK X AND Y AXIS SEPERATLY FOR ACCURATE RESULT
            {
                anim.SetTrigger("grab");
            }
            if (Vector2.Distance(transform.position, player.transform.position) < 1.5f)
            {
                player.GetComponent<PlayerController>().Activate(false);
                hold = true;
                posX = player.transform.position.x - transform.position.x;
            }                
        } else if (hold)
        {
            Vector3 pos = player.transform.position;
            pos.x = (transform.position.x + posX);
            pos.y = transform.position.y;
            player.transform.position = pos;
        }
        
	}

    void Kill ()
    {
        if (hold)
        {
            player.GetComponent<PlayerController>().dead = true;
            player.GetComponent<PlayerController>().isActive = false;
            hold = false;
        }
    }
}
