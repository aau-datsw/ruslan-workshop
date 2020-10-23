FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./CY/*.csproj ./
RUN dotnet restore CY.csproj
COPY ./CY/. ./
RUN dotnet publish CY.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "CY.dll"]