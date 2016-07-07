using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System;

public class UnityAds : AdManager {

    public override void ShowBanner()
    {
        throw new NotImplementedException();
    }

    public override void ShowVideo()
    {
        if (enabled)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            if (inGameTime > videoAdFrequency && Advertisement.IsReady())
            {
                Advertisement.Show();
                inGameTime = 0;
            }
#endif
        }
    }
}
