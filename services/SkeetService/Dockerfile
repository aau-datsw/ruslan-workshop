FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./SkeetAPI/*.csproj ./
RUN dotnet restore SkeetAPI.csproj
COPY ./SkeetAPI/. ./
RUN dotnet publish SkeetAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "SkeetAPI.dll"]