FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./YaAPI/*.csproj ./
RUN dotnet restore YaAPI.csproj
COPY ./YaAPI/. ./
RUN dotnet publish YaAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "YaAPI.dll"]