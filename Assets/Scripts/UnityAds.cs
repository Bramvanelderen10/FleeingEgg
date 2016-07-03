using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAds : MonoBehaviour {

    public float adFrequency = 150;
    public float inGameTime = 0;
    public bool enabled = true;

    public void AddTime(float seconds)
    {
        inGameTime += seconds;
    }

    public void ShowAd()
    {
        if (enabled)
        {
#if UNITY_ANDROID
            if (inGameTime > adFrequency && Advertisement.IsReady())
            {
                Advertisement.Show();
                inGameTime = 0;
            }
#endif
        }

    }
}
