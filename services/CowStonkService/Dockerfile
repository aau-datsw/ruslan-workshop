FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./CowStonk/*.csproj ./
RUN dotnet restore CowStonk.csproj
COPY ./CowStonk/. ./
RUN dotnet publish CowStonk.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "CowStonk.dll"]