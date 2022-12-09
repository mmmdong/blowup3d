using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace MondayOFF {
    public static class IAPManager {
        // Do we really need this? If so, should null check be done?
        public static event Action onBeforePurchase {
            add {
                StoreListener.onBeforePurchase += value;
            }
            remove {
                StoreListener.onBeforePurchase -= value;
            }
        }
        public static event Action<bool> onAfterPurchase {
            add {
                StoreListener.onAfterPurchase += value;
            }
            remove {
                StoreListener.onAfterPurchase -= value;
            }
        }

        private static StoreListener _storeListener = default;

        public static void RegisterProduct(string productID, Action onPurchase) {
            if (_storeListener == null) {
                Debug.Log("[EVERYDAY] Store listener is not created yet");
                return;
            }
            _storeListener.RegisterProducts(productID, onPurchase);
        }

        public static bool PurchaseProduct(string productID) {
            if (_storeListener == null) {
                Debug.Log("[EVERYDAY] Store listener is not created yet");
                return false;
            }
            return _storeListener.PurchaseProduct(productID);
        }

        public static void RestorePurchase() {
            if (_storeListener == null) {
                Debug.Log("[EVERYDAY] Store listener is not created yet");
                return;
            }
            _storeListener.RestorePurchase();
        }

        public async static void Initialze() {
            if (_storeListener != null) {
                Debug.LogWarning("[EVERYDAY] Initialization of IAP Manager is already requested..");
                if (_storeListener.isInitialized) {
                    Debug.LogWarning("[EVERYDAY] IAP Manager is already initialized");
                }
                return;
            }

            Debug.Log("[EVERYDAY] Initializing IAP Manager");

            EverydayProducts everydayProducts = default;
            var assets = Resources.LoadAll<EverydayProducts>("EverydayProducts");
            if (assets == null || assets.Length <= 0) {
                Debug.Log($"[EVERYDAY] {typeof(EverydayProducts).Name} NOT found, search all");
                assets = Resources.LoadAll<EverydayProducts>("");
            }
            if (assets.Length != 1) {
                Debug.LogError($"[EVERYDAY] Found 0 or multiple {typeof(EverydayProducts).Name}s in Resources folder. There should only be one.");
            } else {
                everydayProducts = assets[0];
            }
            Debug.Assert(everydayProducts != null, "[EVERYDAY] EverydayProducts not found!");

            var module = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

            // Add product
            Dictionary<string, ProductData> productDict = new Dictionary<string, ProductData>();
            foreach (var item in everydayProducts.products) {
                builder.AddProduct(item.productID, item.productType, new IDs { { item.productID, AppleAppStore.Name }, { item.productID, GooglePlay.Name } });
                productDict.Add(item.productID, item);
            }

            _storeListener = new StoreListener(productDict);

            try {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                UnityPurchasing.Initialize(_storeListener, builder);

            } catch (System.Exception exception) {
                Debug.LogWarning("[EVERYDAY] Failed to initialize IAP Manager");
                Debug.Log(exception.StackTrace);
            }

#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop() {
            Debug.Log("[EVERYDAY] Stop Playmode IAP Manager");
            _storeListener = null;
        }
#endif
    }
}