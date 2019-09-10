#!/bin/bash

# This script creates a basic, default web API project. 
# If the first argument given to the script is Leaderboards, the project
# will be called LeaderboardsAPI and will be placed in a parent directory 
# called LeaderboardsService. 
# A Dockerfile is created in the parent directory. 

PROJECT_NAME="$1"
SERVICE_NAME=$PROJECT_NAME"Service"
API_NAME=$PROJECT_NAME"API"
DOCKERFILE='
FROM microsoft/dotnet:2.2-sdk AS build\n
WORKDIR /app\n
COPY ./'"$API_NAME"'/*.csproj ./\n
RUN dotnet restore '"$API_NAME"'.csproj\n
COPY ./'"$API_NAME"'/. ./\n
RUN dotnet publish '"$API_NAME"'.csproj -c Release -o pub\n
\n
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime\n
WORKDIR /app\n
COPY --from=0 /app/pub .\n
ENTRYPOINT ["dotnet", "'"$API_NAME"'.dll"]\n
'

# Create the service directory and a Dockerfile
mkdir $SERVICE_NAME && cd $SERVICE_NAME
echo $DOCKERFILE > Dockerfile

# Create the .NET project
dotnet new webapi -o $API_NAME
