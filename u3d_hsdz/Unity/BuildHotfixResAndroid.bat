@echo off

echo ��������Android�ȸ���Դ�ļ�

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath D:\UnityProject\u3d_crazypokers\Unity -quit -batchmode -executeMethod BuildHelper.BuildAndroidApk -logFile build.log
"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleAndroid" -logFile build.log

echo Android�ȸ���Դ�ļ��������

pause
::taskkill /f /im /unity.exe