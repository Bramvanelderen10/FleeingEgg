using UnityEngine;
using System.Collections;
using System;

public class MovingEnviromentController : GameComponent {

    [HideInInspector]
    public float speed = 2f;

    public Rigidbody2D HandsRigidBody;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if (isActive)
        {
            rb2d.velocity = new Vector2(speed, 0);
        } else
        {
            HandsRigidBody.velocity = new Vector2(0, 0);
            rb2d.velocity = new Vector2(0, 0);
        }

        if (transform.position.x > 1000)
        {
            ResetPosition();
        }
    }

    //Resets the position of the enviroment to 0 while all childs have to maintain current position
    //To achieve this all childs objects positions have to be updated on the movement of the enviroment
    public void ResetPosition ()
    {
        float adjustmentValue = transform.position.x;

        Vector3 position = transform.position;
        position.x -= adjustmentValue;
        transform.position = position;

    }
}
