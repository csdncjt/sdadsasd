@echo off

echo 正在生成iOS热更资源文件

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers_ios\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleIOS" -logFile build.log

echo iOS热更资源文件生成完毕

echo 正在拷贝iOS热更资源文件

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers_ios\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.CopyIosResToRelease" -logFile build.log

echo iOS热更资源文件拷贝完毕

pause
::taskkill /f /im /unity.exe