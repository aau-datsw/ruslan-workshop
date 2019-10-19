FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./KauKlanen/*.csproj ./
RUN dotnet restore KauKlanen.csproj
COPY ./KauKlanen/. ./
RUN dotnet publish KauKlanen.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "KauKlanen.dll"]