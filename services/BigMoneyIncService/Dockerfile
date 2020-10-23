FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./BigMoneyInc/*.csproj ./
RUN dotnet restore BigMoneyInc.csproj
COPY ./BigMoneyInc/. ./
RUN dotnet publish BigMoneyInc.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "BigMoneyInc.dll"]