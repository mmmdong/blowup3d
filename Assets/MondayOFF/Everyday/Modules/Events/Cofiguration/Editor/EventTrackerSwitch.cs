using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;

namespace MondayOFF {
    internal class EventTrackerSwitch : AssetPostprocessor {
        const string FIREBASE_ENABLED = "FIREBASE_ENABLED";
        const string CONFIG_FILE_NAME =
#if UNITY_IOS
"GoogleServices-Info.plist"
#else
"google-services.json"
#endif
;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload) {
#if FIREBASE_ENABLED
            foreach (var item in deletedAssets) {
                if (item.Contains(CONFIG_FILE_NAME)) {
                    DidReloadScripts();
                    return;
                }
            }
#else
            foreach (var item in importedAssets) {
                if (item.Contains(CONFIG_FILE_NAME)) {
                    DidReloadScripts();
                    return;
                }
            }
#endif
        }

        [DidReloadScripts(0)]
        private static void DidReloadScripts() {
            // Assembly integrated
            Assembly firebaseAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Firebase.App");
            // Configfiles exist
            bool hasConfigFile = File.Exists(Path.Combine(Application.dataPath, CONFIG_FILE_NAME));
            bool isAdd = firebaseAssembly != null && hasConfigFile;
#if FIREBASE_ENABLED
            if (!isAdd)
#else
            if (isAdd)
#endif
            {
                Debug.Log($"[EVERYDAY] Project has valid Firebase: {isAdd}");
                DefineSymbols.ModifyScriptingDefineSymbol(FIREBASE_ENABLED, isAdd);
            }
        }
    }
}