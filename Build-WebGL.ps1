set mypath=%cd%
@echo %mypath%
Unity.exe -quit -batchmode -logFile stdout.log -projectPath %mypath% -executeMethod WebGLBuilder.Build