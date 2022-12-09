using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayMenuItem {
        [MenuItem("Everyday/Open Everyday Settings", false, 100)]
        private static void FocusAdUnitIDs() {
            var adUnitIDs = AssetDatabase.FindAssets("t:EverydaySettings");

            if (adUnitIDs.Length != 1) {
                Debug.LogError("[EVERYDAY] There are zero or more than two Objects! " + adUnitIDs.Length);
                return;
            }

            UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<EverydaySettings>(AssetDatabase.GUIDToAssetPath(adUnitIDs[0]));
        }
    }
}