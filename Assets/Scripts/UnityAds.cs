using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAds : MonoBehaviour {

    public float adFrequency = 150;
    public float inGameTime = 0;

    public void AddTime(float seconds)
    {
        inGameTime += seconds;
    }

    public void ShowAd()
    {
        if (inGameTime > adFrequency && Advertisement.IsReady())
        {
            Advertisement.Show();
            inGameTime = 0;
        }
    }
}
