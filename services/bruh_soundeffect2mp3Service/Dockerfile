FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./bruh_soundeffect2mp3/*.csproj ./
RUN dotnet restore bruh_soundeffect2mp3.csproj
COPY ./bruh_soundeffect2mp3/. ./
RUN dotnet publish bruh_soundeffect2mp3.csproj -c Release -o pub

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "bruh_soundeffect2mp3.dll"]