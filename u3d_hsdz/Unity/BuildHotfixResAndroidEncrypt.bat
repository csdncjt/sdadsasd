@echo off

echo ��������Android�ȸ���Դ�ļ�(����)

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath D:\UnityProject\u3d_crazypokers\Unity -quit -batchmode -executeMethod BuildHelper.BuildAndroidApk -logFile build.log
REM "C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleAndroidEncrypt" -logFile build.log
"C:\Program Files\Unity\Hub\Editor\2017.4.40c1\Editor\Unity.exe" -projectPath "F:\unity_dezhou1\client\u3d_hsdz\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleAndroidEncrypt" -logFile build.log
echo Android�ȸ���Դ�ļ��������(����)

pause
::taskkill /f /im /unity.exe