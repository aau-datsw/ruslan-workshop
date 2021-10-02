FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Henta1Inc/*.csproj ./
RUN dotnet restore Henta1Inc.csproj
COPY ./Henta1Inc/. ./
RUN dotnet publish Henta1Inc.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Henta1Inc.dll"]