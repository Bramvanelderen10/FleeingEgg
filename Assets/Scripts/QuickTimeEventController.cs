using UnityEngine;
using QuickTimeEvent;

public class QuickTimeEventController : MonoBehaviour
{
    public Type type;
    private Animator anim;

    void Start()
    {
        Debug.Log("started");
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "QTE_Despawn")
        {
            Destroy(transform.gameObject); //Play death animation which then triggers destroy after animation is finished
        }
    }    

    public void TriggerPopUp()
    {
        anim.SetBool("Popup", true);

    }
}



