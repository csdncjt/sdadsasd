@echo off

echo 正在生成Android热更资源文件

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath D:\UnityProject\u3d_crazypokers\Unity -quit -batchmode -executeMethod BuildHelper.BuildAndroidApk -logFile build.log
"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleAndroid" -logFile build.log

echo Android热更资源文件生成完毕

echo 正在生成iOS热更资源文件

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleIOS" -logFile build.log

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.ChangeToAndroidForBat" -logFile build.log

echo iOS热更资源文件生成完毕

pause
::taskkill /f /im /unity.exe