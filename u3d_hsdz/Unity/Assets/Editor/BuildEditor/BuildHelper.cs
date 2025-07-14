using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using UnityEditor;

namespace ETEditor
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

        public static string BuildFolder = "../Release/{0}/StreamingAssets/";

        [MenuItem("Tools/web资源服务器")]
        public static void OpenFileServer()
        {
#if !UNITY_EDITOR_OSX
            string currentDir = System.Environment.CurrentDirectory;
            string path = Path.Combine(currentDir, @"..\FileServer\");
            path = path.Replace('\\', '/');
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "FileServer.exe";
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
#else
			string path = System.Environment.CurrentDirectory + "/../FileServer/";
			("cd " + path + " && go run FileServer.go").Bash(path, true);
#endif
        }



        [MenuItem("Tools/ilrt 复制Hotfix.dll, Hotfix.pdb到Res\\Code")]
        public static void CopyHotfixDll()
        {
            string CodeDir = "Assets/Res/Code/";
            string HotfixDll = "Unity.Hotfix.dll";
            string HotfixPdb = "Unity.Hotfix.pdb";

#if ILRuntime
            // Copy最新的pdb文件
            string[] dirs =
            {
                    "./Temp/UnityVS_bin/Debug",
                    "./Temp/UnityVS_bin/Release",
                    "./Temp/bin/Debug",
                    "./Temp/bin/Release"
            };

            DateTime dateTime = DateTime.MinValue;
            string newestDir = "";
            foreach (string dir in dirs)
            {
                string dllPath = Path.Combine(dir, HotfixDll);
                if (!File.Exists(dllPath))
                {
                    continue;
                }
                FileInfo fi = new FileInfo(dllPath);
                DateTime lastWriteTimeUtc = fi.LastWriteTimeUtc;
                if (lastWriteTimeUtc > dateTime)
                {
                    newestDir = dir;
                    dateTime = lastWriteTimeUtc;
                }
            }

            if (newestDir != "")
            {
                File.Copy(Path.Combine(newestDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
                File.Copy(Path.Combine(newestDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
                Log.Info($"ilrt 复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
            }
#endif
        }

        public static void BuildBundleAndroid()
        {
            BuildTarget buildTarget = BuildTarget.Android;
            // 切换到 Android平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, buildTarget);
            BuildHelper.Build(PlatformType.Android, BuildAssetBundleOptions.None, BuildOptions.None, false, false);
        }

        public static void BuildBundleIOS()
        {
            BuildTarget buildTarget = BuildTarget.iOS;
            // 切换到 iOS平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, buildTarget);
            BuildHelper.Build(PlatformType.IOS, BuildAssetBundleOptions.None, BuildOptions.None, false, false);
        }

        public static void BuildBundleAndroidEncrypt()
        {
            BuildTarget buildTarget = BuildTarget.Android;
            // 切换到 Android平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, buildTarget);
            BuildHelper.Build(PlatformType.Android, BuildAssetBundleOptions.None, BuildOptions.None, false, false, true);
        }

        public static void BuildBundleIOSEncrypt()
        {
            BuildTarget buildTarget = BuildTarget.iOS;
            // 切换到 iOS平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, buildTarget);
            BuildHelper.Build(PlatformType.IOS, BuildAssetBundleOptions.None, BuildOptions.None, false, false, true);
        }

        /// <summary>
        /// 生成AssetBundle
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buildAssetBundleOptions"></param>
        /// <param name="buildOptions"></param>
        /// <param name="isBuildExe"></param>
        /// <param name="isContainAB"></param>
        /// <param name="encrypt">是否加密</param>
        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool encrypt = false)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string exeName = "ET";
            switch (type)
            {
                case PlatformType.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
            }

            string fold = string.Format(BuildFolder, type);
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }

            Log.Info("开始资源打包");
            BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

            GenerateVersionInfo(fold);
            if (encrypt)
            {
                string encryptDir = string.Format(BuildFolder, $"{type}_Encrypt");
                EncryptAssetBundle(fold, $"/{type}/", $"/{type}_Encrypt/");
                GenerateVersionInfo(encryptDir, true);
            }

            Log.Info("完成资源打包");

            if (isContainAB)
            {
                FileHelper.CleanDirectory("Assets/StreamingAssets/");
                FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            }

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/Scenes/Init.unity",
                };
                Log.Info("开始EXE打包");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
                Log.Info("完成exe打包");
            }
        }

        public static void BuildApk()
        {
            string apkName = PlayerSettings.bundleVersion.Replace(".", "_");
            BuildAndroidApk($"allin_{apkName}");
        }

        private static void BuildAndroidApk(string apkName)
        {
            string currentDir = System.Environment.CurrentDirectory;

            BuildTarget buildTarget = BuildTarget.Android;
            // 切换到 Android平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, buildTarget);

            // keystore 路径
            PlayerSettings.Android.keystoreName = $"{currentDir}\\crazypoker.keystore";
            // keystore 密码
            PlayerSettings.Android.keystorePass = "123456";

            // keystore 别名
            PlayerSettings.Android.keyaliasName = "crazypoker";
            // keystore 别名密码
            PlayerSettings.Android.keyaliasPass = "123456";

            List<string> levels = new List<string>();
            foreach (EditorBuildSettingsScene editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                if (!editorBuildSettingsScene.enabled)
                    continue;

                levels.Add(editorBuildSettingsScene.path);
            }

            // Apk名
            string dstName = string.Empty;
            if (string.IsNullOrEmpty(apkName))
            {
                dstName = "game.apk";
            }
            else
            {
                dstName = $"{apkName}.apk";
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = levels.ToArray();
            // buildPlayerOptions.locationPathName = dstName;
            // buildPlayerOptions.locationPathName = $"{currentDir}\\..\\Release\\{dstName}";
            buildPlayerOptions.locationPathName = $"{relativeDirPrefix}\\{dstName}";
            buildPlayerOptions.target = buildTarget;
            buildPlayerOptions.options = BuildOptions.None;

            // 打包
            string res = BuildPipeline.BuildPlayer(buildPlayerOptions);

            AssetDatabase.Refresh();
        }

        public static void CopyIosResToRelease()
        {
            string targetProjectDirName = "u3d_crazypokers";
            // D:\UnityProject\u3d_crazypokers\Unity
            // D:\UnityProject\u3d_crazypokers_ios\Unity

            string currentDir = System.Environment.CurrentDirectory.Replace('\\', '/');

            string sourceDir = $"{currentDir}/../Release/IOS";
            string targetDir = $"{currentDir}/../../{targetProjectDirName}/Release/IOS";

            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);

            if (source.FullName.Equals(target.FullName))
            {
                UnityEngine.Debug.LogError($"sourceDir targetDir is same! dir:{source.FullName}");
                return;
            }


            if (Directory.Exists(sourceDir))
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                else
                {
                    Directory.Delete(targetDir, true);
                    Directory.CreateDirectory(targetDir);
                }

                FileHelper.CopyDirectory(sourceDir, targetDir);
            }
            else
            {
                UnityEngine.Debug.LogError($"sourceDir: {sourceDir} not exists");
                return;
            }
            UnityEngine.Debug.Log($"Copy Success! sourceDir:{sourceDir}, targetDir:{targetDir}");
        }

        public static void CopyIosResToReleaseEncrypt()
        {
            string targetProjectDirName = "u3d_crazypokers";

            string currentDir = System.Environment.CurrentDirectory.Replace('\\', '/');

            string sourceDir = $"{currentDir}/../Release/IOS_Encrypt";
            string targetDir = $"{currentDir}/../../{targetProjectDirName}/Release/IOS_Encrypt";

            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);

            if (source.FullName.Equals(target.FullName))
            {
                UnityEngine.Debug.LogError($"sourceDir targetDir is same! dir:{source.FullName}");
                return;
            }


            if (Directory.Exists(sourceDir))
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                else
                {
                    Directory.Delete(targetDir, true);
                    Directory.CreateDirectory(targetDir);
                }

                FileHelper.CopyDirectory(sourceDir, targetDir);
            }
            else
            {
                UnityEngine.Debug.LogError($"sourceDir: {sourceDir} not exists");
                return;
            }
            UnityEngine.Debug.Log($"Copy Success! sourceDir:{sourceDir}, targetDir:{targetDir}");
        }

        /// <summary>
        /// 生成资源匹配文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pIsABSec">是否加密</param>
        public static void GenerateVersionInfo(string dir, bool pIsABSec = false)
        {
            VersionConfig versionProto = new VersionConfig();
            GenerateVersionProto(dir, versionProto, "");

            versionProto.ABEncrypt = pIsABSec ? 1 : 0;  // 0不加密 1加密

            using (FileStream fileStream = new FileStream($"{dir}/Version.txt", FileMode.Create))
            {
                byte[] bytes = JsonHelper.ToJson(versionProto).ToByteArray();
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void GenerateVersionProto(string dir, VersionConfig versionProto, string relativePath)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                string md5 = MD5Helper.FileMD5(file);
                FileInfo fi = new FileInfo(file);
                long size = fi.Length;
                string filePath = relativePath == "" ? fi.Name : $"{relativePath}/{fi.Name}";

                versionProto.FileInfoDict.Add(filePath, new FileVersionInfo
                {
                    File = filePath,
                    MD5 = md5,
                    Size = size,
                });
            }
            foreach (string directory in Directory.GetDirectories(dir))
            {
                DirectoryInfo dinfo = new DirectoryInfo(directory);
                string rel = relativePath == "" ? dinfo.Name : $"{relativePath}/{dinfo.Name}";
                GenerateVersionProto($"{dir}/{dinfo.Name}", versionProto, rel);
            }
            //string tDayHour = DateTime.Now.ToString("yyMMddHHmm");
            string tDayHour = DateTime.Now.ToString("yyMMddHHmm");
            versionProto.Version = long.Parse(tDayHour);
        }

        public static void EncryptAssetBundle(string dir, string oldKey, string newKey)
        {
            List<string> files = new List<string>();
            FileHelper.GetAllFiles(files, dir);

            string tarDir = dir.Replace(oldKey, newKey);
            if (Directory.Exists(tarDir))
            {
                Directory.Delete(tarDir, true);
            }

            Directory.CreateDirectory(tarDir);

            string tmpFile = string.Empty;
            foreach (string file in files)
            {
                if (!file.EndsWith(".manifest"))
                {
                    tmpFile = file.Replace(oldKey, newKey);
                    if (file.EndsWith(".txt"))
                    {
                        byte[] oldData = File.ReadAllBytes(file);
                        FileHelper.WriteFile(tmpFile, oldData);
                    }
                    else
                    {
                        byte[] oldData = File.ReadAllBytes(file);
                        int newDataLength = 128 + oldData.Length;
                        var newData = new byte[newDataLength];
                        for (int i = 0, n = oldData.Length; i < n; i++)
                        {
                            newData[128 + i] = oldData[i];
                        }

                        for (int i = 0, n = 128; i < n; i++)
                        {
                            newData[i] = (byte)i;
                        }
                        FileHelper.WriteFile(tmpFile, newData);
                    }
                }
            }
        }
    }
}