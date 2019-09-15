FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./MatchmakingAPI/*.csproj ./
RUN dotnet restore MatchmakingAPI.csproj
COPY ./MatchmakingAPI/. ./
RUN dotnet publish MatchmakingAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "MatchmakingAPI.dll"]