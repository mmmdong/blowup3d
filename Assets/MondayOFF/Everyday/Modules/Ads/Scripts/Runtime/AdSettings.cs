using UnityEngine;

namespace MondayOFF {
    [System.Serializable]
    internal class AdSettings {
        [Header("Initialize Ads Manager on load")]
        [Tooltip("If you want to manually initialize Ads Manager (ex/ to check No Ads status before initialization), do NOT forget to initialize Ads Manager manually.")]
        [SerializeField] internal bool initializeOnLoad = true;

        [Header("Show Banner upon initialization")]
        [SerializeField] internal bool showBannerOnLoad = false;

        [Header("Ad initialization order")]
        [SerializeField] internal AdInitializationOrder adInitializationOrder = AdInitializationOrder.Banner_Inter_Reward;
        [SerializeField][Range(0f, 10f)] internal float delay = 2f;
        [Header("Interstitial Interval")]
        [Tooltip("Minimum delay between interstitial ads")]
        [SerializeField][Range(5f, 120f)] internal float interstitialInterval = 45f;
        [Tooltip("Should interstitial timer reset after rewarded ad?")]
        [SerializeField] internal bool resetTimerOnRewarded = false;
        [Header("Banner Position")]
        [SerializeField] internal MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;

#pragma warning disable CS0414
        [Header("====== iOS Ad Unit IDs ======")]
        [SerializeField][LabelOverride("Interstitial (IS)")] bool iOS_has_IS = false;
        [SerializeField][DrawIf("iOS_has_IS", "Ad Unit ID")] string iOS_IS_AdUnitID = "";
        [SerializeField][LabelOverride("Rewarded (RV)")] bool iOS_has_RV = false;
        [SerializeField][DrawIf("iOS_has_RV", "Ad Unit ID")] string iOS_RV_AdUnitID = "";
        [SerializeField][LabelOverride("Banner (BN)")] bool iOS_has_BN = false;
        [SerializeField][DrawIf("iOS_has_BN", "Ad Unit ID")] string iOS_BN_AdUnitID = "";

        [Header("====== Android Ad Unit IDs ======")]
        [SerializeField][LabelOverride("Interstitial (IS)")] bool AOS_has_IS = false;
        [SerializeField][DrawIf("AOS_has_IS", "Ad Unit ID")] string AOS_IS_AdUnitID = "";
        [SerializeField][LabelOverride("Rewarded (RV)")] bool AOS_has_RV = false;
        [SerializeField][DrawIf("AOS_has_RV", "Ad Unit ID")] string AOS_RV_AdUnitID = "";
        [SerializeField][LabelOverride("Banner (BN)")] bool AOS_has_BN = false;
        [SerializeField][DrawIf("AOS_has_BN", "Ad Unit ID")] string AOS_BN_AdUnitID = "";

        [Space(20)]
        [Header("====== PlayOn ======")]
        [SerializeField][LabelOverride("PlayOn")] internal bool hasPlayOn = false;
        [SerializeField][DrawIf("hasPlayOn", "Android Api Key")] private string playOnAPIKey_Android = "";
        [Space(15)]
        [SerializeField][DrawIf("hasPlayOn", "iOS Api Key")] private string playOnAPIKey_iOS = "";
        [SerializeField][DrawIf("hasPlayOn", "Apple AppStore ID")] internal string storeID = "";
        [Tooltip("Anchor position of logo ad")]
        [Space(15)]
        [SerializeField][DrawIf("hasPlayOn", "Logo Anchor Position", order = 2)] internal PlayOnSDK.Position playOnLogoAnchor = PlayOnSDK.Position.TopLeft;
        [Tooltip("Offset from anchor")]
        [SerializeField][DrawIf("hasPlayOn", "Logo Offset")] internal Vector2Int playOnLogoOffset = new Vector2Int(0, 150);
        [Tooltip("Logo ad size")]
        [SerializeField][DrawIf("hasPlayOn", "Logo Size")] internal int playOnLogoSize = 64;
#pragma warning restore CS0414

#if UNITY_IOS
        internal string interstitialAdUnitId => iOS_IS_AdUnitID;
        internal string rewardedAdUnitId => iOS_RV_AdUnitID;
        internal string bannerAdUnitId => iOS_BN_AdUnitID;

        internal bool hasInterstitial => iOS_has_IS;
        internal bool hasRewarded => iOS_has_RV;
        internal bool hasBanner => iOS_has_BN;

        internal string playOnAPIKey => playOnAPIKey_iOS;
#else
        internal string interstitialAdUnitId => AOS_IS_AdUnitID;
        internal string rewardedAdUnitId => AOS_RV_AdUnitID;
        internal string bannerAdUnitId => AOS_BN_AdUnitID;

        internal bool hasInterstitial => AOS_has_IS;
        internal bool hasRewarded => AOS_has_RV;
        internal bool hasBanner => AOS_has_BN;

        internal string playOnAPIKey => playOnAPIKey_Android;
#endif

        internal bool HasInterstitialAdUnitID() {
            return hasInterstitial && !string.IsNullOrEmpty(interstitialAdUnitId);
        }

        internal bool HadRewardedAdUnitID() {
            return hasRewarded && !string.IsNullOrEmpty(rewardedAdUnitId);
        }

        internal bool HasBannerAdUnitID() {
            return hasBanner && !string.IsNullOrEmpty(bannerAdUnitId);
        }
    }
}