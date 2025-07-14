using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCodePostProcess
{

#if UNITY_EDITOR
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild( BuildTarget target, string pathToBuiltProject )
    {
#if UNITY_5
        if (target != BuildTarget.iOS) {
#else
        if (target != BuildTarget.iOS) {
#endif
            Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        //得到xcode工程的路径
        string path = Path.GetFullPath(pathToBuiltProject);

        // Create a new project object from build target
        XCProject project = new XCProject( pathToBuiltProject );

        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        string[] files = Directory.GetFiles( Application.dataPath, "*.projmods", SearchOption.AllDirectories );
        foreach( string file in files ) {
            UnityEngine.Debug.Log("ProjMod File: "+file);
            project.ApplyMod( file );
        }

        //TODO disable the bitcode for iOS 9
        project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Release");
        project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Debug");

        //TODO implement generic settings as a module option
        //        project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution", "Release");

        //编辑代码文件
        EditorCode(path);

        // Finally save the xcode project
        project.Save();

    }

    private static void EditorCode(string filePath)
    {
        //读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

        //在指定代码后面增加一行代码
        UnityAppController.WriteBelow("#import <OpenGLES/ES2/glext.h>", "#import \"JPUSHService.h\"\n#import \"JPushEventCache.h\"\n#import <UserNotifications/UserNotifications.h>");
        UnityAppController.WriteBelow("- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions\n{", "    [application setApplicationIconBadgeNumber:0];\n    [[JPushEventCache sharedInstance] handFinishLaunchOption:launchOptions];\n    [JPUSHService setupWithOption:launchOptions appKey:@\"28db22f05ba924071c9a46d0\" channel:@\"\" apsForProduction:NO];");
        UnityAppController.WriteBelow("UnitySendDeviceToken(deviceToken);", "    [JPUSHService registerDeviceToken:deviceToken];");
        UnityAppController.WriteBelow("- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo\n{", "    [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@\"JPushPluginReceiveNotification\"];\n    [JPUSHService handleRemoteNotification:userInfo];");
        UnityAppController.WriteBelow("- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler\n{", "    [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@\"JPushPluginReceiveNotification\"];");

        UnityAppController.WriteBelow("    ::printf(\"-> applicationDidEnterBackground()\\n\");", "    self.bgTask = [application beginBackgroundTaskWithExpirationHandler:^{\n        dispatch_async(dispatch_get_main_queue(), ^{\n            NSLog(@\"结束\");\n            if (self.bgTask != UIBackgroundTaskInvalid)\n            {\n                [application endBackgroundTask:self.bgTask];\n                self.bgTask = UIBackgroundTaskInvalid;\n            }\n        });\n    }];\n\n    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{\n\n        NSTimeInterval timeRemain = 0;\n        do{\n            [NSThread sleepForTimeInterval:1];\n            if (self.bgTask!= UIBackgroundTaskInvalid) {\n                timeRemain = [application backgroundTimeRemaining];\n                NSLog(@\"Time remaining: %f\",timeRemain);\n            }\n        }while(self.bgTask!= UIBackgroundTaskInvalid && timeRemain > 0);\n\n        dispatch_async(dispatch_get_main_queue(), ^{\n\n            if (self.bgTask != UIBackgroundTaskInvalid)\n            {\n                [application endBackgroundTask:self.bgTask];\n            }\n        });\n    });");
        UnityAppController.WriteBelow("    ::printf(\"-> applicationWillEnterForeground()\\n\");", "    //如果没到10分钟又打开了app,结束后台任务\n    if (self.bgTask != UIBackgroundTaskInvalid) {\n        [application endBackgroundTask:self.bgTask];\n        self.bgTask = UIBackgroundTaskInvalid;\n    }");

        XClass UnityAppControllerHeader = new XClass(filePath + "/Classes/UnityAppController.h");
        UnityAppControllerHeader.WriteBelow("@property (nonatomic, copy)                                 void(^quitHandler)();", "@property (nonatomic) UIBackgroundTaskIdentifier bgTask;");

        XClass UnityAppControllerRendering = new XClass(filePath + "/Classes/UnityAppController+Rendering.mm");
        //在指定代码中替换一行
        UnityAppControllerRendering.Replace("    if (!_didResignActive)\n    {\n        [self repaint];\n        [self processTouchEvents];\n    }", "    [self repaint];\n    [self processTouchEvents];");
        UnityAppControllerRendering.Replace("    if (!UnityIsPaused())", "    //if (!UnityIsPaused())");

        XClass PreprocessorHeader = new XClass(filePath + "/Classes/Preprocessor.h");
        PreprocessorHeader.Replace("#define UNITY_SNAPSHOT_VIEW_ON_APPLICATION_PAUSE 1", "#define UNITY_SNAPSHOT_VIEW_ON_APPLICATION_PAUSE 0");
    }
#endif

    public static void Log(string message)
    {
        UnityEngine.Debug.Log("PostProcess: "+message);
    }
}
