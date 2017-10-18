
set name="%1"
set src=%~2
set pub=%~2..\..\..\..\pub\
set archiv=%pub%\com-kit_%APPVEYOR_BUILD_NUMBER%.zip

echo Copy from %src%
echo Copy to %pub%

if not exist "%pub%" mkdir "%pub%"

:: xcopy "$(TargetDir)*" "$(SolutionDir)..\pub\$(TargetName)\" /D /Y /S

 ::xcopy "%src%*.exe" "%pub%" /D /Y /S
 ::xcopy "%src%*.dll" "%pub%" /D /Y /S
 ::xcopy "%src%*.config" "%pub%" /D /Y /S

 ::xcopy "%src%*.ids" "%pub%" /D /Y /S

pushd "%src%"

7z a "%archiv%" "*.exe"
7z a "%archiv%" "*.dll"
7z a "%archiv%" "*.config"

7z a "%archiv%" "*.ids" -r

popd
