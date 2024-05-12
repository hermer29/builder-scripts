using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.BuildTarget;

namespace BuilderScript.Editor
{
    public static class Builder
    {
        [MenuItem("Builds/🕸Build WebGL")]
        public static void BuildWebGl()
        {
            const BuildTarget Platform = WebGL;
            
            PredefinePlatformSpecificSettings(Platform);
        
            EditorPrefs.SetInt("Addressables.BuildAddressablesWithPlayerBuild", 1);
            
            var buildFolderPath = CustomCommandLineArgs.GetArgumentValueEnsureNotNull("build-folder-path");
            
            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = buildFolderPath,
                scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray(),
                target = Platform
            });
        }

        [MenuItem("Builds/🤖Build Android")]
        public static void BuildAndroidApk()
        {
            const BuildTarget Platform = Android;

            PredefinePlatformSpecificSettings(Platform);

            EditorPrefs.SetInt("Addressables.BuildAddressablesWithPlayerBuild", 1);

            var buildFolderPath = CustomCommandLineArgs.GetArgumentValueEnsureNotNull("build-folder-path");

            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = buildFolderPath,
                scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray(),
                target = Platform
            });
        }

        private static void PredefinePlatformSpecificSettings(BuildTarget target)
        {
            if(target == WebGL)
                PlayerSettings.colorSpace = ColorSpace.Gamma;

            if(target == Android)
            {
                var ndkPath = CustomCommandLineArgs.GetArgumentValueEnsureNotNull("android-ndk-path");
                    Debug.Log($"Set into AndroidNdkRootR16b value: {ndkPath}");
                if(ndkPath != null)
                {
                    EditorPrefs.SetString("AndroidNdkRootR16b", ndkPath);
                    EditorPrefs.SetString("AndroidNdkRootR19", ndkPath);
                    EditorPrefs.SetString("AndroidNdkRoot", ndkPath);
                }
                var sdkPath = CustomCommandLineArgs.GetArgumentValueEnsureNotNull("android-sdk-path");
                    Debug.Log($"Set into AndroidSdkRoot value: {sdkPath}");
                if(sdkPath != null)
                {
                    EditorPrefs.SetString("AndroidSdkRoot", sdkPath);
                }
            }
        }
    }
}
