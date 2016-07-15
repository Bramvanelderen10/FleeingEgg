using UnityEngine;
using System.Collections;
using QuickTimeEvent;
using System.Collections.Generic;

public class PlayerController : GameComponent {

    public bool controlTypeNext = false;
    public bool dead = false;
    //private Rigidbody2D rb2dParent;
    //private MovingEnviromentController mec;

    public float m_speed = 2f;
    public float m_current_speed = 2f;

    private bool isMoving = true;    

    private Transform target = null;
    private Type currentTargetType = Type.None;
    private Type nextTargetType = Type.None;

    private int hits = 0;
    private int misses = 0;

    public void Activate(bool activate = true)
    {
        isActive = activate;
        isMoving = true;
        target = null;
        currentTargetType = Type.None;
        nextTargetType = Type.None;

        if (activate)
        {
            hits = 0;
            misses = 0;
        }
    }
	
    void Update()
    {
        if (!isActive)
            return;

        if (target == null)
        {
            target = FindNextQTE(transform, true);
        }

        //if ismoving move towards the target
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, m_current_speed * Time.deltaTime);
            if (transform.position == target.transform.position)
            {
                isMoving = false;

                Transform newTarget = FindNextQTE(target);
                target = (newTarget) ? newTarget : target;
            }

        } else if (target != null)
        {

            //Prevent pressing all buttons at once
            int pressCount = 0;
            foreach (Type enumValue in System.Enum.GetValues(typeof(Type)))
            {
                if (enumValue != Type.None && GetButtonDown(QuickTimeEvent.Utils.ConvertTypeToString(enumValue)))
                {
                    pressCount++;
                }
            }
            if (pressCount > 2)
            {
                misses++;
            }

            if (currentTargetType != Type.None && GetButtonDown(QuickTimeEvent.Utils.ConvertTypeToString(currentTargetType)))
            {
                SoundManager.Instance.PlaySFX(SoundManager.Type.Hit);
                hits++;
                isMoving = true;
                target.GetComponent<QuickTimeEventController>().TriggerPopUp();
            }
        } else
        {
            Transform newTarget = FindNextQTE(target);
            target = (newTarget) ? newTarget : target;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        //Vector3 position = transform.position;

        //rb2d.velocity = new Vector2(m_speed, 0);
        //rb2d.velocity = new Vector2(rb2dParent.velocity.x + mec.backwardVelocity + .010001f, 0);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy") {
            dead = true; //Gamemanager can detect based on the dead value if the player is still alive if not the gamemanager triggers the destroy
            this.gameObject.SetActive(false); //Play death animation which then triggers destroy after animation is finished
        }            
    }

    public Transform FindNextQTE(Transform start, bool firstTime = false)
    {
        Transform newTarget = null;
        foreach (GameObject gameobject in GameObject.FindGameObjectsWithTag("QTE"))
        {
            float xDistance = gameobject.transform.position.x - start.position.x;

            //The new target may not be in the same position or to the left of the current position
            //This will filter both the current qte and all qte's to the left
            if (xDistance > 0f)
            {
                //If newtarget is already defined from previous loops then compare the  distance of the newTarget transfrom to the current loop
                //if the distance is smaller of the current loop then the newTarget this one will override the newTarget
                //Since all QTE's on the left side are already filtered we dont have to think about negative distances here                    
                if (newTarget)
                {
                    float gameObjectDistance = gameobject.transform.position.x - start.position.x;
                    float newTargetDistacne = newTarget.position.x - start.position.x;
                    if (gameObjectDistance < newTargetDistacne)
                    {
                        newTarget = gameobject.transform;
                    }
                }
                else
                {
                    newTarget = gameobject.transform;
                }
            }
        }
        if (newTarget == null)
            return newTarget;

        Type newType = newTarget.gameObject.GetComponent<QuickTimeEventController>().type;
        if (firstTime)
        {
            currentTargetType = nextTargetType = newType;
        } else
        {
            if (controlTypeNext)
            {

                newTarget.GetComponent<QuickTimeEventController>().TriggerPopUp();
                target.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                currentTargetType = nextTargetType = newType;
            } else
            {
                currentTargetType = nextTargetType;
                nextTargetType = newType;
            }
                    
        }        

        return newTarget;
    }

    public void SetSpeed(float speed)
    {
        this.m_speed = m_current_speed = speed;
    }    

    public int GetMisses()
    {

        return misses;
    }

    public int GetHits()
    {

        return hits;
    }

    public void AddMiss()
    {
        misses++;
    }

    private bool GetButtonDown(string type)
    {
        bool result = false;
        result = (Input.GetButtonDown(type) || MobileInput.Instance.GetButtonDown(type));

        return result;
    }
}
