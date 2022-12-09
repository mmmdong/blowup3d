using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;

namespace MondayOFF {
    internal static class DefineSymbols {
        internal static void ModifyScriptingDefineSymbol(string symbol, bool isAdd) {
            BuildTargetGroup[] groups = new BuildTargetGroup[]{
                BuildTargetGroup.iOS, BuildTargetGroup.Android
            };

            foreach (var currentGroup in groups) {
                bool hasChanged = false;
                var definedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup);
                var defines = new List<string>(definedSymbols.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                if (isAdd) {
                    if (!defines.Contains(symbol)) {
                        defines.Add(symbol);
                        hasChanged = true;
                    }
                } else {
                    if (defines.Contains(symbol)) {
                        defines.Remove(symbol);
                        hasChanged = true;
                    }
                }

                if (hasChanged) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, string.Join(";", defines.ToArray()));
                    CompilationPipeline.RequestScriptCompilation();
                }
            }
        }
    }
}