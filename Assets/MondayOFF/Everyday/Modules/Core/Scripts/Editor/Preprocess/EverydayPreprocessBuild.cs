using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Linq;

namespace MondayOFF {
    public class EverydayPreprocessBuild : IPreprocessBuildWithReport {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            var settingAssets = AssetDatabase.FindAssets("t:EverydaySettings");

            if (settingAssets.Length != 1) {
                throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] There should be ONLY 1 settings object.");
            }

            var settings = AssetDatabase.LoadAssetAtPath<EverydaySettings>(AssetDatabase.GUIDToAssetPath(settingAssets[0]));
            AddSettingsToPreload(settings);
            ValidateAdUnitID(settings);

            if (string.IsNullOrEmpty(Facebook.Unity.Settings.FacebookSettings.ClientToken)) {
                var fbSettings = AssetDatabase.FindAssets(filter: "t:FacebookSettings");

                if (fbSettings.Length != 1) {
                    Debug.LogError("[EVERYDAY] There are zero or more than two Objects! " + fbSettings.Length);
                    return;
                }

                UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<Facebook.Unity.Settings.FacebookSettings>(AssetDatabase.GUIDToAssetPath(fbSettings[0]));
                throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] Facebook Client Token is empty!.");
            }

            // Make sure Sprite Atlasing is enabled?
            if ((int)UnityEditor.EditorSettings.spritePackerMode < (int)SpritePackerMode.BuildTimeOnlyAtlas) {
                UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnlyAtlas;
            }

#if UNITY_PRO_LICENSE
            UnityEditor.PlayerSettings.SplashScreen.showUnityLogo = false;
#else
            UnityEditor.PlayerSettings.SplashScreen.animationMode = UnityEditor.PlayerSettings.SplashScreen.AnimationMode.Static;
            UnityEditor.PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.LightOnDark;
            UnityEditor.PlayerSettings.SplashScreen.backgroundColor = new Color32(0x1E, 0x1E, 0x1E, 0xff);
#endif
        }

        private void AddSettingsToPreload(EverydaySettings everydaySettings) {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            foreach (var asset in preloadedAssets) {
                if (asset.GetType() == typeof(EverydaySettings)) {
                    Debug.Log("[EVERYDAY] EverydaySettigns is in preload asset");
                    return;
                }
            }

            var list = preloadedAssets.ToList();
            list.Add(everydaySettings);

            PlayerSettings.SetPreloadedAssets(list.ToArray());
            Debug.Log("[EVERYDAY] Adding EverydaySettigns to preload assets");
        }

        private void ValidateAdUnitID(EverydaySettings settings) {
            if (!settings.ValidateAdUnitIDs()) {
                if (!EditorUtility.DisplayDialog("EVERYDAY\nMissing Ad Unit ID", "Some of ad placements are enabled but missing Ad Unit ID!\nAre you sure you want to continue build?", "Continue", "Cancel")) {
                    UnityEditor.Selection.activeObject = settings;
                    throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] Please check Ad Unit IDs.");
                }
            }
        }
    }
}