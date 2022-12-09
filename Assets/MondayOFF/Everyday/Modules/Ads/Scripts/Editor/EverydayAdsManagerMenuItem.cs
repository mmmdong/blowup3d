using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayAdsManagerMenuItem : MonoBehaviour {
        [MenuItem("Everyday/!! Strip AdMob !!", false, 500)]
        private static void StripFirebase() {
            string[] FirebaseAssets = new string[]{
                "Assets/MaxSdk/Mediation/Google",
                "Assets/Plugins/Android/MaxMediationGoogle.androidlib",
            };

            bool hasDeleted = false;
            foreach (var item in FirebaseAssets) {
                if (Directory.Exists(item)) {
                    if (Directory.EnumerateFileSystemEntries(item).Any()) {
                        Directory.Delete(item, true);
                        hasDeleted = true;
                    } else {
                        Directory.Delete(item, false);
                    }
                    var metaPath = item + ".meta";
                    if (File.Exists(metaPath)) {
                        File.Delete(item + ".meta");
                        hasDeleted = true;
                    }
                }
            }

            if (hasDeleted) {
                AssetDatabase.Refresh();
            }
        }
    }
}