FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./gyldenkaal_inc/*.csproj ./
RUN dotnet restore gyldenkaal_inc.csproj
COPY ./gyldenkaal_inc/. ./
RUN dotnet publish gyldenkaal_inc.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "gyldenkaal_inc.dll"]