using UnityEngine;
using System.Collections;
using admob;
using System;

public class GoogleAds : AdManager {

    public string bannedId;
    public string videoId;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        Admob.Instance().initAdmob(bannedId, videoId);
        Admob.Instance().loadInterstitial();
#endif
    }

    public override void ShowBanner()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 5);
#endif
    }

    public override void ShowVideo()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        if (inGameTime > videoAdFrequency && Admob.Instance().isInterstitialReady()) 
        {
            Admob.Instance().showInterstitial();
            inGameTime = 0;
        }
#endif
    }
}
