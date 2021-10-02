FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./JeffBezos/*.csproj ./
RUN dotnet restore JeffBezos.csproj
COPY ./JeffBezos/. ./
RUN dotnet publish JeffBezos.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "JeffBezos.dll"]