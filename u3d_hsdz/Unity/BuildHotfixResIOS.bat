@echo off

echo ��������iOS�ȸ���Դ�ļ�

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers_ios\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.BuildBundleIOS" -logFile build.log

echo iOS�ȸ���Դ�ļ��������

echo ���ڿ���iOS�ȸ���Դ�ļ�

"C:\Program Files\Unity\Editor\Unity.exe" -projectPath "D:\UnityProject\u3d_crazypokers_ios\Unity" -quit -batchmode -executeMethod "ETEditor.BuildHelper.CopyIosResToRelease" -logFile build.log

echo iOS�ȸ���Դ�ļ��������

pause
::taskkill /f /im /unity.exe