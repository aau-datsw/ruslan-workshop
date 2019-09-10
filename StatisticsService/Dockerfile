FROM microsoft/dotnet:2.2-sdk AS build
 WORKDIR /app
 COPY ./StatisticsAPI/*.csproj ./
 RUN dotnet restore StatisticsAPI.csproj
 COPY ./StatisticsAPI/. ./
 RUN dotnet publish StatisticsAPI.csproj -c Release -o pub
 
 FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
 WORKDIR /app
 COPY --from=0 /app/pub .
 ENTRYPOINT ["dotnet", "StatisticsAPI.dll"]

