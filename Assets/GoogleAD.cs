using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public static class GoogleAD {

	private static string adUnitId = "ca-app-pub-4278269334769006/7451421575";

	private static BannerView bannerView;
	private static AdRequest request;

	public static void showAd(){
		showAd(true);
	}

	public static void showAd(bool top){
		if(bannerView == null){
			bannerView = new BannerView(adUnitId, AdSize.Banner, ((top) ? AdPosition.Top : AdPosition.Bottom));
			request = new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
		} else {
			bannerView.Show ();
		}
	}

	public static void hideAd(){
		if(bannerView != null)
			bannerView.Hide();
	}


}
