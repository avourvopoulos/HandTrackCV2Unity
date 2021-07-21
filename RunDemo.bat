@ECHO OFF
set mypath=%cd%
START %mypath%\Builds\Builds.exe
TIMEOUT 0
python %mypath%\Python_Client\openCVhandtrack.py