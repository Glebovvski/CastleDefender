using System;
using GoogleMobileAds.Api;
using Models;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AdManager : IInitializable
    {
        private const string noAdsKey = "NoAds"; // 0 = show ads; 1 = no ads

        private GameControlModel GameControlModel { get; set; }

        private BannerView bannerView;
        private InterstitialAd interstitialAd;

        public event Action OnCanShowAdValueChanged;
        private bool canShowAd = true;
        public bool CanShowAd
        {
            get => canShowAd;
            private set
            {
                PlayerPrefs.SetInt(noAdsKey, value ? 0 : 1);
                bool newValue = PlayerPrefs.GetInt(noAdsKey, 0) < 1;
                canShowAd = newValue;
                OnCanShowAdValueChanged?.Invoke();
            }
        }
        [Inject]
        private void Construct(GameControlModel gameControlModel)
        {
            GameControlModel = gameControlModel;
        }

        public void Initialize()
        {
            OnCanShowAdValueChanged += HideBanner;
            if (!CanShowAd) return;

            MobileAds.Initialize(initStatus => { });
            RequestInterstitial();
            RequestBanner();
            GameControlModel.OnRestart += ShowInterstitialAd;
        }

        public void SetNoAds() => CanShowAd = false;

        private void ShowInterstitialAd()
        {
            if(interstitialAd == null) return;
            
            if (interstitialAd.CanShowAd() && CanShowAd)
                interstitialAd.Show();
        }

        private void RequestBanner()
        {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2564312810103530/8287739858";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-2564312810103530/7469269205";
#endif

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
            bannerView.Show();
        }

        private void RequestInterstitial()
        {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2564312810103530/3111453302";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-2564312810103530~3588582562";
#else
        string adUnitId = "unexpected_platform";
#endif

            AdRequest request = new AdRequest.Builder().Build();
            InterstitialAd.Load(adUnitId, request, InterstitialAdCallback);
        }

        private void InterstitialAdCallback(InterstitialAd ad, LoadAdError error)
        {
            if (ad == null) return;

            interstitialAd = ad;
            interstitialAd.OnAdFullScreenContentClosed += RequestInterstitial;
            interstitialAd.OnAdFullScreenContentClosed += HandleInterstitialClosed;
        }

        public event Action OnInterstitialAdClosed;
        private void HandleInterstitialClosed()
        {
            interstitialAd.OnAdFullScreenContentClosed -= RequestInterstitial;
            interstitialAd.OnAdFullScreenContentClosed -= HandleInterstitialClosed;

            OnInterstitialAdClosed?.Invoke();
        }

        private void HideBanner()
        {
            if (bannerView == null) return;

            bannerView.Hide();
            bannerView.Destroy();
        }
    }
}