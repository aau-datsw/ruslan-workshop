FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./duenInc/*.csproj ./
RUN dotnet restore duenInc.csproj
COPY ./duenInc/. ./
RUN dotnet publish duenInc.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "duenInc.dll"]