FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./FittyFitty/*.csproj ./
RUN dotnet restore FittyFitty.csproj
COPY ./FittyFitty/. ./
RUN dotnet publish FittyFitty.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "FittyFitty.dll"]