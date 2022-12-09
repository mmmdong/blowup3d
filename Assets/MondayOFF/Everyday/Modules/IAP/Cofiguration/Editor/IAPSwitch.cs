using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Reflection;
using System;
using System.IO;
using System.Linq;

namespace MondayOFF {
    internal class IAPSwitch : AssetPostprocessor {
        const string UNITY_PURCHASING = "UNITY_PURCHASING";

#if UNITY_PURCHASING
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload) {
            // It only breaks when removing package
            foreach (var item in deletedAssets) {
                if (item.Contains("com.unity.purchasing")) {
                    Debug.Log($"[EVERYDAY] Removing Unity Purchasing from project");
                    DefineSymbols.ModifyScriptingDefineSymbol(UNITY_PURCHASING, false);
                    return;
                }
            }
        }
#endif

        [DidReloadScripts(0)]
        private static void DidReloadScripts() {
            // Assembly integrated
            Assembly unityPurchasingAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "UnityEngine.Purchasing");
            // Configfiles exist
            bool isAdd = unityPurchasingAssembly != null;
#if UNITY_PURCHASING
            if (!isAdd)
#else
            if (isAdd)
#endif
            {
                Debug.Log($"[EVERYDAY] Project has UnityEngine.Purchasing: {isAdd}");
                DefineSymbols.ModifyScriptingDefineSymbol(UNITY_PURCHASING, isAdd);
            }
        }
    }
}