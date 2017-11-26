using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class Build
{
    [MenuItem("Build/Build Sender and Receiver")]
    public static void BuildReceiver()
    {
        PlayerSettings.productName = "QS-UDP-Tester-Receiver";
        PlayerSettings.companyName = "QuiteSensible";

        BuildPipeline.BuildPlayer(new[] { "Assets/receiver.unity" },
            System.IO.Path.GetFullPath(Application.dataPath + "/../Receiver.exe"),
            BuildTarget.StandaloneWindows64,
            BuildOptions.AutoRunPlayer);

        PlayerSettings.productName = "QS-UDP-Tester-Sender";
        PlayerSettings.companyName = "QuiteSensible";

        PrepareAndroid();

        BuildPipeline.BuildPlayer(
            new[] { "Assets/main.unity" },
            "udp_test.apk",
            BuildTarget.Android,
            BuildOptions.AutoRunPlayer);
    }

    private static void PrepareAndroid()
    {
        int buildID = PlayerSettings.Android.bundleVersionCode;
        buildID++;

        PlayerSettings.Android.bundleVersionCode = buildID;
        PlayerSettings.bundleVersion = string.Format("0.{0:d4}", buildID);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.quitesensible.udp_test");
        PlayerSettings.productName = "udp_test";
    }
}
