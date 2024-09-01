@echo off
@echo **********************
@echo Clean up app outputs
@echo **********************

FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

del *.bak /s
RMDIR /Q/S publish
RMDIR /Q/S Release

@echo Search for the devenv path!
set folder="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE"
IF EXIST %folder% (
    @echo find Visual Studio - 2019 - Community
    set devEnvpath="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv"
    set msbuildEnvpath="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild"   
) ELSE (
    set folder="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE"

    IF EXIST %folder% (
    @echo find Visual Studio - 2019 - Professional
    set devEnvpath="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv"  
    set msbuildEnvpath="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild"   
    ) ELSE (  
        set folder="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE"
        IF EXIST %folder% (
           @echo find Visual Studio - 2017 - Community
           set devEnvpath ="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv"
           set msbuildEnvpath="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Current\Bin\msbuild"   
        ) ELSE (
          @echo **********************
          @echo Could not find Visual Studio
          @echo **********************
          goto Error
        )     
    )     
 )

@echo **********************
@echo Clean solutin
@echo **********************
%devEnvpath% ".\src\Authorization.sln" /Clean

@echo **********************
@echo restore packages
@echo **********************
dotnet restore ".\src\Authorization.sln"

@echo **********************
@echo Rebuild solutin
@echo **********************
%devEnvpath% ".\src\Authorization.sln" /rebuild Release

@echo **********************
@echo Deploy solutin
@echo **********************
dotnet publish -c Release --self-contained false --output .\publish ".\src\Authorization.WebApi\Authorization.WebApi.csproj"


del .\publish\*.pdb

:Success:
goto End

:Error:

:End

