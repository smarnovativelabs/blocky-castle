using UnityEngine;
using System.Collections;
using admob;

public class AdMob : MonoBehaviour {

	public string AppleBannerID;
	public string AppleVideoID;
	public string AndroidBannerID;
	public string AndroidVideoID;

	public static AdMob o;

	void Start ()
	{
		if (o == null) 
			o = this;
		else 
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
        AndroidBannerID = "";
        AndroidVideoID = "";
		AppleBannerID = "";
		AppleVideoID = "ca-app-pub-8367138567926894/5727902163";
        //Admob.Instance ().setTesting (true);
        #if UNITY_IOS
		Admob.Instance ().initAdmob (AppleBannerID, AppleVideoID);
#endif
#if UNITY_ANDROID
        Admob.Instance ().initAdmob (AndroidBannerID, AndroidVideoID);
		#endif
		Admob.Instance ().loadInterstitial ();
	}

	public void ShowBanner () {
		Admob.Instance ().showBannerRelative (AdSize.Banner, AdPosition.TOP_CENTER, 5);
		print ("Banner Ad Displayed");
	}

	public void DestroyBanner () {
		Admob.Instance ().removeBanner ();
		print ("Banner Ad Destroyed");
	}

	public void ShowVideo () {
		if (Admob.Instance ().isInterstitialReady ()) {
			Admob.Instance ().showInterstitial ();
			print ("Video Ad Displayed");
		} else 
			print ("Video Ad is Loading...");
		StartCoroutine (LoadVideo ());
	}

	private IEnumerator LoadVideo () {
		yield return new WaitForSeconds (1);
		Admob.Instance ().loadInterstitial();
	}
}
