FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./Brams/*.csproj ./
RUN dotnet restore Brams.csproj
COPY ./Brams/. ./
RUN dotnet publish Brams.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Brams.dll"]