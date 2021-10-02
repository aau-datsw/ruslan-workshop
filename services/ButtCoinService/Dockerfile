FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./ButtCoin/*.csproj ./
RUN dotnet restore ButtCoin.csproj
COPY ./ButtCoin/. ./
RUN dotnet publish ButtCoin.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "ButtCoin.dll"]