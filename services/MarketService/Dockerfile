FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./MarketAPI/*.csproj ./
RUN dotnet restore MarketAPI.csproj
COPY ./MarketAPI/. ./
RUN dotnet publish MarketAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "MarketAPI.dll"]