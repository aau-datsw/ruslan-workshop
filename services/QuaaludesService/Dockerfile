FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./Quaaludes/*.csproj ./
RUN dotnet restore Quaaludes.csproj
COPY ./Quaaludes/. ./
RUN dotnet publish Quaaludes.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Quaaludes.dll"]