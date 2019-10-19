FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./Q8/*.csproj ./
RUN dotnet restore Q8.csproj
COPY ./Q8/. ./
RUN dotnet publish Q8.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "Q8.dll"]