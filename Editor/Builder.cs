using System;
using System.IO;
using System.Linq;
using System.Globalization;
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
            var now = DateTime.Now;
            var culture = new CultureInfo("ru-RU");
    
            var buildFolderName = $"{now.ToString("dd.MM.yyyy", culture)}_{PlayerSettings.productName}_{now.ToString("hh.mm", culture)}";
            
            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = $"{GetArtifactsFolderLocation()}/{buildFolderName}",
                scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray(),
                target = Platform
            });
        }

        [MenuItem("Builds/🤖Build Android")]
        public static void BuildAndroidApk()
        {
            const BuildTarget Platform = Android;

            PredefinePlatformSpecificSettings(Platform);

            var now = DateTime.Now;
            var culture = new CultureInfo("ru-RU");
    
            var buildFileName = $"{now.ToString("dd.MM.yyyy", culture)}_{PlayerSettings.productName}_{now.ToString("hh.mm", culture)}.apk";
            
            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = $"{GetArtifactsFolderLocation()}/{buildFileName}",
                scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray(),
                target = Platform
            });
        }

        private static string? GetParameterValue(string parameterName)
        {
            var args = Environment.GetCommandLineArgs();
            return args.FirstOrDefault(x => x.StartsWith($"--{parameterName}="))?
                .Split($"--{parameterName}=").Last();
        }

        private static string GetArtifactsFolderLocation()
        {
            if(Directory.Exists("../../src"))
            {
                return "../../artifacts";
            }
            return "./artifacts";
        }

        private static void PredefinePlatformSpecificSettings(BuildTarget target)
        {
            if(target == WebGL)
                PlayerSettings.colorSpace = ColorSpace.Gamma;

            if(target == Android)
            {
                var ndkPath = GetParameterValue("Android_Ndk_Path");
                    Debug.Log($"Set into AndroidNdkRootR16b value: {ndkPath}");
                if(ndkPath != null)
                {
                    EditorPrefs.SetString("AndroidNdkRootR16b", ndkPath);
                    EditorPrefs.SetString("AndroidNdkRootR19", ndkPath);
                    EditorPrefs.SetString("AndroidNdkRoot", ndkPath);
                }
                var sdkPath = GetParameterValue("Android_Sdk_Path");
                    Debug.Log($"Set into AndroidSdkRoot value: {sdkPath}");
                if(sdkPath != null)
                {
                    EditorPrefs.SetString("AndroidSdkRoot", sdkPath);
                }
            }
        }
    }
}
