#!/bin/bash

# This script creates a basic, default web API project. 
# If the first argument given to the script is Leaderboards, the project
# will be called LeaderboardsAPI and will be placed in a parent directory 
# called LeaderboardsService. 
# A Dockerfile is created in the parent directory. 

PROJECT_NAME="$1"
SERVICE_NAME=$PROJECT_NAME"Service"
API_NAME=$PROJECT_NAME"API"
DATABASE_NAME=$(echo "$PROJECT_NAME" | tr '[:upper:]' '[:lower:]')-db

# Create the service directory and a Dockerfile
mkdir $SERVICE_NAME && cd $SERVICE_NAME
cat <<EOF > Dockerfile
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./${API_NAME}/*.csproj ./
RUN dotnet restore ${API_NAME}.csproj
COPY ./${API_NAME}/. ./
RUN dotnet publish ${API_NAME}.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "${API_NAME}.dll"]
EOF


# Create the .NET project
dotnet new webapi -o $API_NAME

# Create the database seed
mkdir dbscripts && cd dbscripts
cat <<EOF > seed.sql
\connect ${DATABASE_NAME}

CREATE TABLE people (
  id          SERIAL       PRIMARY KEY,
  name        TEXT,
  other_name  TEXT
) WITH (OIDS=FALSE);

ALTER TABLE people OWNER TO ya;

INSERT INTO people(name, other_name) VALUES(
  'Anders Brams',
  'The literal god'
);
EOF

