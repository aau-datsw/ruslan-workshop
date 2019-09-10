FROM microsoft/dotnet:2.2-sdk AS build
 WORKDIR /app
 COPY ./TournamentAPI/*.csproj ./
 RUN dotnet restore TournamentAPI.csproj
 COPY ./TournamentAPI/. ./
 RUN dotnet publish TournamentAPI.csproj -c Release -o pub
 
 FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
 WORKDIR /app
 COPY --from=0 /app/pub .
 ENTRYPOINT ["dotnet", "TournamentAPI.dll"]

