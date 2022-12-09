﻿using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

namespace MondayOFF {
    public static partial class AdsManager {
        private static bool _isInitialized = false;

        private static AdSettings _settings = default;
        private static AdType _activeAdTypes = AdType.All;

        private static Interstitial _interstitial = default;
        private static Rewarded _rewarded = default;
        private static Banner _banner = default;
        private static PlayOn _playOn = default;

#if UNITY_EDITOR
        private static CancellationTokenSource _source = default;
#endif

        internal static void PrepareManager(AdSettings settings) {
            Debug.Log($"[EVERYDAY] Preparing Ads Manager");
            _settings = settings;

            // This is to handle Initialize() call before preparation
            if (_isInitialized && !_settings.initializeOnLoad) {
                InitializeAdTypes();
            }
#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop() {
            Debug.Log("[EVERYDAY] Stop Playmode Ads Manager");
            _source?.Cancel();
            _source?.Dispose();
            _source = null;

            _interstitial?.Dispose();
            _interstitial = null;
            _rewarded?.Dispose();
            _rewarded = null;
            _banner?.Dispose();
            _banner = null;
            _playOn?.Dispose();
            _playOn = null;

            var methodInfo = typeof(MaxSdk).GetMethod("RemoveReadyAdUnit", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            methodInfo?.Invoke(null, new[] { _settings.bannerAdUnitId });
            methodInfo?.Invoke(null, new[] { _settings.interstitialAdUnitId });
            methodInfo?.Invoke(null, new[] { _settings.rewardedAdUnitId });

            _isInitialized = false;
        }
#endif

        private static async void InitializeAdTypes() {
            if (_settings == null) {
                Debug.Log($"[EVERYDAY] Ads Manager is not prepared. It will continue initialization after settings are loaded.");
                return;
            }
            System.TimeSpan delay = System.TimeSpan.FromSeconds(_settings.delay);
            System.Func<bool>[] adLoadingFuncs = new System.Func<bool>[3]{
                CreateBanner,
                CreateInterstitial,
                CreateRewarded
            };
            ushort loadingOrder = (ushort)_settings.adInitializationOrder;
            System.Func<bool> createAd = adLoadingFuncs[(loadingOrder >> 7) & 0b11];

#if UNITY_EDITOR
            _source = new CancellationTokenSource();
            CancellationToken token = _source.Token;
            try {
                if (createAd.Invoke()) {
                    Debug.Log($"[EVERYDAY] First ad type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                createAd = adLoadingFuncs[(loadingOrder >> 4) & 0b11];
                if (createAd.Invoke()) {
                    Debug.Log($"[EVERYDAY] Second Ad Type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                createAd = adLoadingFuncs[(loadingOrder >> 1) & 0b11];
                if (createAd.Invoke()) {
                    Debug.Log($"[EVERYDAY] Third Ad Type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                CreatePlayOn();
            } catch (System.Exception) {
                Debug.Log("[EVERYDAY] Application stopped, stop initializing ads");
            } finally {
                _source.Dispose();
                _source = null;
            }
#else
            if (createAd.Invoke()) {
                Debug.Log($"[EVERYDAY] First ad type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            createAd = adLoadingFuncs[(loadingOrder >> 4) & 0b11];
            
            if (createAd.Invoke()) {
                Debug.Log($"[EVERYDAY] Second Ad Type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            createAd = adLoadingFuncs[(loadingOrder >> 1) & 0b11];
            
            if (createAd.Invoke()) {
                Debug.Log($"[EVERYDAY] Third Ad Type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            CreatePlayOn();
#endif
        }

        private static bool CreateBanner() {
            if (_banner != null) {
                Debug.LogWarning("[EVERYDAY] Banner is already initialized!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Banner) || !_settings.HasBannerAdUnitID()) {
                Debug.Log("[EVERYDAY] Skipping banner initialization");
                return false;
            }

            _banner = new Banner(_settings, _settings.showBannerOnLoad);
            return true;
        }

        private static bool CreateInterstitial() {
            if (_interstitial != null) {
                Debug.LogWarning("[EVERYDAY] Interstitial is already initialized!!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Interstitial) || !_settings.HasInterstitialAdUnitID()) {
                Debug.Log("[EVERYDAY] Skipping intersitial initialization");
                return false;
            }

            _interstitial = new Interstitial(_settings, _settings.interstitialInterval);
            return true;
        }

        private static bool CreateRewarded() {
            if (_rewarded != null) {
                Debug.LogWarning("[EVERYDAY] Rewarded is already initialized!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Rewarded) || !_settings.HadRewardedAdUnitID()) {
                Debug.Log("[EVERYDAY] Skipping rewarded initialization");
                return false;
            }

            _rewarded = new Rewarded(_settings);
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += AdsManager.InternalLoadedCallback;
            return true;
        }

        private static void CreatePlayOn() {
            if (_playOn != null) {
                Debug.LogWarning("[EVERYDAY] PlayOn is already initialized!");
                return;
            }

            if (!_settings.hasPlayOn) {
                Debug.Log("[EVERYDAY] Skipping PlayOn initialization");
                return;
            }

            _playOn = new PlayOn(_settings);
        }

        private static void InternalLoadedCallback(string _, MaxSdk.AdInfo __) {
            OnRewardedAdLoaded?.Invoke();
        }

        private static bool CheckInitialization() {
            if (_isInitialized) {
                return false;
            }
            Debug.Log("[EVERYDAY] Ads Manager is NOT initialized!");
            return true;
        }
    }
}