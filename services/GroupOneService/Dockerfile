FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./GroupOne/*.csproj ./
RUN dotnet restore GroupOne.csproj
COPY ./GroupOne/. ./
RUN dotnet publish GroupOne.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "GroupOne.dll"]