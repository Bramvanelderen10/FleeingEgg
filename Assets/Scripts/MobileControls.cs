using UnityEngine;
using System.Collections;

public class MobileControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.gameObject.SetActive(false);
#if UNITY_ANDROID
        transform.gameObject.SetActive(true);
#endif
    }

    public void SetActive(bool active)
    {
        transform.gameObject.SetActive(false);
#if UNITY_ANDROID
        transform.gameObject.SetActive(active);
#endif
    }
}
