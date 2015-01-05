using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public static class GoogleAD {

	private static string adUnitId = "ca-app-pub-4278269334769006/7451421575";

	private static BannerView bannerView;
	private static AdRequest request;

	private static bool isOpen = false;

	public static void showAd(){
		showAd(true);
	}

	public static void showAd(bool top){
		if(bannerView == null){
			bannerView = new BannerView(adUnitId, AdSize.Banner, ((top) ? AdPosition.Top : AdPosition.Bottom));
			request = new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
			isOpen = true;
		}
		if(!isOpen){			
			bannerView.Show();
			isOpen = true;
		}
	}

	public static void hideAd(){
		if(isOpen){
			if(bannerView != null)
				bannerView.Hide();
			isOpen = false;
		}
	}

}
