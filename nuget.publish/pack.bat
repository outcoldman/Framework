msbuild.exe ..\sources\Framework.sln /property:Configuration=Release /m 
sn -R "..\bin\Release\Portable\OutcoldSolutions.Framework.dll" %OutcoldTools%\OutcoldSolutions.snk
sn -R "..\bin\Release\Windows\OutcoldSolutions.Presentation.dll" %OutcoldTools%\OutcoldSolutions.snk
sn -R "..\bin\Release\Suites\OutcoldSolutions.SuitesBase.dll" %OutcoldTools%\OutcoldSolutions.snk
nuget pack "..\sources\Portable\OutcoldSolutions.Framework\OutcoldSolutions.Framework.csproj" -basepath "..\bin\Release\Portable" -prop Configuration=Release
nuget pack "..\sources\Windows\OutcoldSolutions.Presentation\OutcoldSolutions.Presentation.csproj" -basepath "..\bin\Release\Windows" -prop Configuration=Release
nuget pack "..\sources\Suites\OutcoldSolutions.SuitesBase\OutcoldSolutions.SuitesBase.csproj" -basepath "..\bin\Release\Suites" -prop Configuration=Release