FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./NillerGang/*.csproj ./
RUN dotnet restore NillerGang.csproj
COPY ./NillerGang/. ./
RUN dotnet publish NillerGang.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "NillerGang.dll"]