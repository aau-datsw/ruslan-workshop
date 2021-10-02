FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Balle/*.csproj ./
RUN dotnet restore Balle.csproj
COPY ./Balle/. ./
RUN dotnet publish Balle.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Balle.dll"]