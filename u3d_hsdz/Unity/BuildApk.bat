@echo off

echo ��������Apk�ļ�

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath D:\UnityProject\u3d_crazypokers\Unity -quit -batchmode -executeMethod BuildHelper.BuildAndroidApk -logFile build.log
"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildApk" -logFile build.log

::%1 -projectPath %2 -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildApk" -logFile build.log

echo Apk�ļ��������
pause
::taskkill /f /im /unity.exe