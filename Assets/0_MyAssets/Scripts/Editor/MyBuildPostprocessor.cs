#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class MyBuildPostprocessor : IPreprocessBuildWithReport
{
    // 実行順
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
    }

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        string projectPath = PBXProject.GetPBXProjectPath(path);

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        //Exception: Calling TargetGuidByName with name='Unity-iPhone' is deprecated.【解決策】
        //https://koujiro.hatenablog.com/entry/2020/03/16/050848
        string target = pbxProject.GetUnityMainTargetGuid();


        //pbxProject.AddCapability(target, PBXCapabilityType.InAppPurchase);

        // Plistの設定のための初期化
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        //日付とか
        string dateName = DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2");
        string timeName = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2");

        if (Debug.isDebugBuild)
        {
            //アプリ名
            plist.root.SetString("CFBundleDisplayName", $"{dateName}_debug");

            //bundleId
            pbxProject.SetBuildProperty(target, "PRODUCT_BUNDLE_IDENTIFIER", Application.identifier + ".dev");
        }

        //ipa名
        string buildMode = Debug.isDebugBuild ? "debug" : "release";
        string name = $"{Application.productName}_{buildMode}_ver{Application.version}_{dateName}_{timeName}";
        // Debug.Log($"~~~~~~~~~~~~~~~\n{name}\n~~~~~~~~~~~~~~~");

        plist.WriteToFile(plistPath);
        pbxProject.WriteToFile(projectPath);
    }


}
#endif