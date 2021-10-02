FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Backup2/*.csproj ./
RUN dotnet restore Backup2.csproj
COPY ./Backup2/. ./
RUN dotnet publish Backup2.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Backup2.dll"]