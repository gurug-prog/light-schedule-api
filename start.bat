@ECHO OFF

SET "APP_CERT_PATH=%USERPROFILE%\.aspnet\https"

SET "CLEAN_CRT=F"
IF "%1"=="--clean" ( SET "CLEAN_CRT=T" )
IF "%2"=="--clean" ( SET "CLEAN_CRT=T" )

IF "%CLEAN_CRT%"=="T" (
    DEL "%APP_CERT_PATH%\appcert.pfx"
)

IF NOT EXIST "%APP_CERT_PATH%\appcert.pfx" (
    dotnet dev-certs https --clean
    dotnet dev-certs https -ep %APP_CERT_PATH%\appcert.pfx -p Certific@t3!_Password123
    dotnet dev-certs https --trust
) ELSE (
    ECHO "Certificate already exists!"
)

SET "RUN_BUILD=F"
IF "%1"=="--build" ( SET "RUN_BUILD=T" )
IF "%2"=="--build" ( SET "RUN_BUILD=T" )

IF "%RUN_BUILD%"=="T" (
    docker compose up -d --build
) ELSE (
    docker compose up -d
)
