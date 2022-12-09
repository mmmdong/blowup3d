using UnityEngine;

namespace MondayOFF {
    public static partial class AdsManager {
        public static event System.Action OnBeforeInterstitial {
            add { Interstitial.OnBeforeShow += value; }
            remove { Interstitial.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterInterstitial {
            add { Interstitial.OnAfterShow += value; }
            remove { Interstitial.OnAfterShow -= value; }
        }
        public static event System.Action OnBeforeRewarded {
            add { Rewarded.OnBeforeShow += value; }
            remove { Rewarded.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterRewarded {
            add { Rewarded.OnAfterShow += value; }
            remove { Rewarded.OnAfterShow -= value; }
        }
        public static event System.Action OnRewardedAdLoaded = default;
        public static string US_PRIVACY_STRING = "";
        public static bool HAS_USER_CONSENT = false;

        public static void Initialize(AdType activeAdTypes = AdType.All) {
            if (_isInitialized) {
                Debug.LogWarning("[EVERYDAY] AdsManager is already initialized!");
                return;
            }

            Debug.Log("[EVERYDAY] Initializing AdsManager..");
            _isInitialized = true;
            _activeAdTypes = activeAdTypes;

            InitializeAdTypes();
        }

        public static void ShowBanner() {
            if (CheckInitialization()) { return; }

            if (_banner == null) {
                if (_settings != null) {
                    _settings.showBannerOnLoad = true;
                }
                Debug.Log("[EVERYDAY] ShowBanner is called but banner ad is not created or has been destroyed");
                return;
            }

            _banner.Show();
        }

        public static void HideBanner() {
            if (CheckInitialization()) { return; }

            if (_banner == null) {
                if (_settings != null) {
                    _settings.showBannerOnLoad = false;
                }
                Debug.Log("[EVERYDAY] HideBanner is called but banner ad is not created or has been destroyed");
                return;
            }

            _banner.Hide();
        }

        public static bool ShowInterstitial() {
            if (CheckInitialization()) { return false; }

            if (_interstitial == null) {
                Debug.Log("[EVERYDAY] ShowInterstitial is called but interstitial ad is not created or has been destroyed");
                return false;
            }

            return _interstitial.Show();
        }

        public static bool IsRewardedReady() {
            if (CheckInitialization()) { return false; }

            if (_rewarded == null) {
                Debug.Log("[EVERYDAY] IsRewardedReady is called but rewarded ad is not created or has been destroyed");
                return false;
            }

            return _rewarded.IsReady();
        }

        public static bool ShowRewarded(System.Action rewardCallback) {
            if (CheckInitialization()) { return false; }

            if (_rewarded == null) {
                Debug.Log("[EVERYDAY] ShowRewarded is called but rewarded ad is not created or has been destroyed!");
                return false;
            }

            if (rewardCallback != null) {
                _rewarded.SetReward(rewardCallback);
                return _rewarded.Show();
            } else {
                Debug.Log("[EVERYDAY] Rewarding callback is not set properly!");
                return false;
            }
        }

        public static void ShowPlayOn() {
            if (CheckInitialization()) { return; }

            if (_playOn == null) {
                Debug.Log("[EVERYDAY] ShowPlayOn is called but PlayOn is not created or has been destroyed");
                return;
            }
            if (_playOn.IsReady()) {
                _playOn.Show();
            }
        }

        public static void HidePlayOn() {
            if (CheckInitialization()) { return; }

            if (_playOn == null) {
                Debug.Log("[EVERYDAY] HidePlayOn is called but PlayOn is not created or has been destroyed");
                return;
            }

            if (_playOn.IsReady()) {
                _playOn.Hide();
            }
        }

        public static void DisableAdType(AdType adType) {
            if (CheckInitialization()) { return; }

            if (adType.HasFlag(AdType.Banner)) {
                DisableBanner();
            }
            if (adType.HasFlag(AdType.Rewarded)) {
                DisableRewarded();
            }
            if (adType.HasFlag(AdType.Interstitial)) {
                DisableInterstitial();
            }
        }

        public static void DisableBanner() {
            if (CheckInitialization()) { return; }

            if (_banner != null) {
                _banner.Dispose();
                _banner = null;
                _activeAdTypes &= ~AdType.Banner;
            }
        }

        public static void DisableInterstitial() {
            if (CheckInitialization()) { return; }

            if (_interstitial != null) {
                _interstitial.Dispose();
                _interstitial = null;
                _activeAdTypes &= ~AdType.Interstitial;
            }
        }

        public static void DisableRewarded() {
            if (CheckInitialization()) { return; }

            if (_rewarded != null) {
                _rewarded.Dispose();
                _rewarded = null;
                _activeAdTypes &= ~AdType.Rewarded;
                MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= AdsManager.InternalLoadedCallback;
            }
        }
    }
}