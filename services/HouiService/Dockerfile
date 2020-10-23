FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./Houi/*.csproj ./
RUN dotnet restore Houi.csproj
COPY ./Houi/. ./
RUN dotnet publish Houi.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Houi.dll"]