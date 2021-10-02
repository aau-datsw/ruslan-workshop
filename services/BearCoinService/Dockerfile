FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./BearCoin/*.csproj ./
RUN dotnet restore BearCoin.csproj
COPY ./BearCoin/. ./
RUN dotnet publish BearCoin.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "BearCoin.dll"]