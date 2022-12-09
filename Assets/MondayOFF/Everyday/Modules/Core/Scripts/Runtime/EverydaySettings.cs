using UnityEngine;

namespace MondayOFF {
    internal class EverydaySettings : ScriptableObject {
        [Header("Enable verbose logging")]
        [SerializeField] internal bool verboseLogging = false;

        [Header("Ad Settings")]
        [SerializeField] internal AdSettings adSettings = default;
#if UNITY_EDITOR
        internal bool ValidateAdUnitIDs() {
            bool isValid = true;
            if (adSettings.hasInterstitial) {
                isValid &= !string.IsNullOrEmpty(adSettings.interstitialAdUnitId);
            }
            if (adSettings.hasRewarded) {
                isValid &= !string.IsNullOrEmpty(adSettings.rewardedAdUnitId);
            }
            if (adSettings.hasBanner) {
                isValid &= !string.IsNullOrEmpty(adSettings.bannerAdUnitId);
            }
            if (adSettings.hasPlayOn) {
                isValid &= !string.IsNullOrEmpty(adSettings.playOnAPIKey);
                isValid &= !string.IsNullOrEmpty(adSettings.storeID);
            }
            return isValid;
        }
#endif
    }
}