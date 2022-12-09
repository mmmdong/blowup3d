using UnityEngine;
using System.Reflection;

namespace MondayOFF {
    public static partial class EveryDay {
        private static EverydaySettings _settings = default;

        public static void SetVerboseLogging(bool isVerbose) {
            _settings.verboseLogging = isVerbose;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AfterAssembliesLoaded() {
            LoadEverydaySettings();
            PrepareLogger();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad() {
            Debug.Log("[EVERYDAY] Initialize..");
            Initialize();
        }

        private static void LoadEverydaySettings() {
            var assets = Resources.LoadAll<EverydaySettings>("EverydaySettings");
            if (assets == null || assets.Length <= 0) {
                Debug.Log("NOT found, search all");
                assets = Resources.LoadAll<EverydaySettings>("");
            }
            if (assets.Length != 1) {
                Debug.LogError($"[EVERYDAY] Found 0 or multiple {typeof(EverydaySettings).Name}s in Resources folder. There should only be one.");
            } else {
                _settings = assets[0];
            }
            Debug.Assert(_settings != null, "NO SETTINGS FOUND");
        }

        private static void Initialize() {
            // MaxSDK
            Debug.Log("[EVERYDAY] Initializing MaxSDK");
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnMaxSdkInitialized;
            MaxSdkCallbacks.OnSdkInitializedEvent += OnMaxSdkInitialized;
            MaxSdk.SetSdkKey("-uBAP4IJbzlOMFq-KJUwdvW8bwGdhtGgmRr9V8T65CUSSIQocwhFqCNP7e2pVITFkJPERuLW5q-X7PJlJ_-7CM");
            MaxSdk.InitializeSdk();
        }

        private static void OnMaxSdkInitialized(MaxSdk.SdkConfiguration sdkConfiguration) {
            PrepareSettings(sdkConfiguration);

            // Initialize Facebook
            FacebookInitializer.Initialize();

            // Initialize Singular
            FieldInfo instanceField = typeof(SingularSDK).GetField("instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            SingularSDK instance = (SingularSDK)instanceField.GetValue(null);
            if (instance == null) {
                var singularGO = new GameObject("SingularSDKObject", typeof(SingularSDK));
                GameObject.DontDestroyOnLoad(singularGO);
            }

            // Send Max AdInfo to Singular
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;

            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;

            // Initialize Ads Manager
            AdsManager.PrepareManager(_settings.adSettings);
            if (_settings.adSettings.initializeOnLoad) {
                AdsManager.Initialize();
            }

            // Initialize Events Manager
            EventTracker.Initialize();

#if UNITY_PURCHASING
            // Initialze IAP Manager if possible
            IAPManager.Initialze();
#endif
        }
    }
}