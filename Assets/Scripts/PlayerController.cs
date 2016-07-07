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

    private bool hasMissed = false;

    public void Activate(bool activate = true)
    {
        isActive = activate;
        isMoving = true;
        target = null;
        currentTargetType = Type.None;
        nextTargetType = Type.None;
        hasMissed = false;

        if (activate)
        {
            hits = misses = 0;
        }
    }

    // Use this for initialization
    void Start () {
        //rb2dParent = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        //mec = transform.parent.gameObject.GetComponent<MovingEnviromentController>();

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
            if (!hasMissed && Input.GetButtonDown(QuickTimeEvent.Utils.ConvertTypeToString(currentTargetType)))
            {
                //m_current_speed = m_current_speed / 3;
                misses++;
                hasMissed = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.position, m_current_speed * Time.deltaTime);
            if (transform.position == target.transform.position)
            {
                //target.GetComponent<QuickTimeEventController>().TriggerPopUp();
                isMoving = false;
                
                //Recover lost speed mechanic
                //Every time the player presses a button a partial of the max speed is recovered
                if  (m_current_speed < m_speed)
                {
                    m_current_speed += (m_speed / 6);
                }

                if (m_current_speed > m_speed)
                {
                    m_current_speed = m_speed;
                }
                Transform newTarget = FindNextQTE(target);
                target = (newTarget) ? newTarget : target;
            }

        } else if (target != null)
        {

            //Prevent pressing all buttons at once
            int pressCount = 0;
            foreach (Type enumValue in System.Enum.GetValues(typeof(Type)))
            {
                if (enumValue != Type.None && Input.GetButtonDown(QuickTimeEvent.Utils.ConvertTypeToString(enumValue)))
                {
                    pressCount++;
                }
            }
            if (pressCount > 1)
            {
                misses++;
            }

            if (currentTargetType != Type.None && Input.GetButtonDown(QuickTimeEvent.Utils.ConvertTypeToString(currentTargetType)))
            {
                SoundManager.Instance.PlaySFX(SoundManager.Type.Hit);
                hits++;
                hasMissed = false;
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

    //This method is a simulation of what would happen with a normal keypress but instead using unity buttons
    public void OnClick(Type type)
    {        
        if (!isActive)
            return;

        
        if (isMoving)
        {           
            if (!hasMissed && type == currentTargetType)
            {
                hasMissed = true;
                misses++;
                //m_current_speed = m_current_speed / 3;
            }            
        }
        else if (target != null)
        {
            if (currentTargetType != Type.None && type == currentTargetType)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Type.Hit);
                hasMissed = false;
                hits++;
                isMoving = true;
                if (!controlTypeNext)
                    target.GetComponent<QuickTimeEventController>().TriggerPopUp();
            }
        }
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
}
