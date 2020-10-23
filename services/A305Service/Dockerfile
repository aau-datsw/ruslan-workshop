FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./A305/*.csproj ./
RUN dotnet restore A305.csproj
COPY ./A305/. ./
RUN dotnet publish A305.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "A305.dll"]