FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Backup1/*.csproj ./
RUN dotnet restore Backup1.csproj
COPY ./Backup1/. ./
RUN dotnet publish Backup1.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Backup1.dll"]