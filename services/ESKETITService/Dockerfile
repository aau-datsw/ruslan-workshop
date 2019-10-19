FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./ESKETIT/*.csproj ./
RUN dotnet restore ESKETIT.csproj
COPY ./ESKETIT/. ./
RUN dotnet publish ESKETIT.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "ESKETIT.dll"]