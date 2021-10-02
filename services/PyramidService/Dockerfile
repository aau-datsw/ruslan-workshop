FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Pyramid/*.csproj ./
RUN dotnet restore Pyramid.csproj
COPY ./Pyramid/. ./
RUN dotnet publish Pyramid.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Pyramid.dll"]