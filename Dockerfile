FROM microsoft/dotnet:2.2-sdk AS build
 WORKDIR /app
 COPY ./LeaderboardAPI/*.csproj ./
 RUN dotnet restore LeaderboardAPI.csproj
 COPY ./LeaderboardAPI/. ./
 RUN dotnet publish LeaderboardAPI.csproj -c Release -o pub
 
 FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
 WORKDIR /app
 COPY --from=0 /app/pub .
 ENTRYPOINT ["dotnet", "LeaderboardAPI.dll"]

