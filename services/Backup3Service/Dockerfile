FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Backup3/*.csproj ./
RUN dotnet restore Backup3.csproj
COPY ./Backup3/. ./
RUN dotnet publish Backup3.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Backup3.dll"]