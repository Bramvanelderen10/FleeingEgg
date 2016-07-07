using UnityEngine;
using System.Collections;

public abstract class AdManager : MonoBehaviour {

    public static AdManager Instance { set; get; }

    public float videoAdFrequency = 150;
    public float inGameTime = 0;

    public bool enabled = true;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void AddTime(float seconds)
    {
        inGameTime += seconds;
    }

    public abstract void ShowBanner();
    public abstract void ShowVideo();    
}
