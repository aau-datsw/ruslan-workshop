FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./YeetAPI/*.csproj ./
RUN dotnet restore YeetAPI.csproj
COPY ./YeetAPI/. ./
RUN dotnet publish YeetAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "YeetAPI.dll"]