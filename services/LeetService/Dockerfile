FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./Leet/*.csproj ./
RUN dotnet restore Leet.csproj
COPY ./Leet/. ./
RUN dotnet publish Leet.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Leet.dll"]