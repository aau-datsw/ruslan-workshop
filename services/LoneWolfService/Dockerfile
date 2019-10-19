FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./LoneWolf/*.csproj ./
RUN dotnet restore LoneWolf.csproj
COPY ./LoneWolf/. ./
RUN dotnet publish LoneWolf.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "LoneWolf.dll"]