FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./AccountAPI/*.csproj ./
RUN dotnet restore AccountAPI.csproj
COPY ./AccountAPI/. ./
RUN dotnet publish AccountAPI.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "AccountAPI.dll"]
