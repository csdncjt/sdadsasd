@echo off

::未测试

echo 正在构建Hotfix项目

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe" "D:\UnityProject\u3d_crazypokers\Unity\Unity.sln" /Rebuild "Debug|AnyCPU" /Out "BuildDll.log"

echo Hotfix项目构建完毕

pause