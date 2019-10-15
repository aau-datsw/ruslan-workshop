#!/bin/bash
docker-compose -f config/docker-compose-local.yml down

docker-compose -f config/docker-compose-local.yml build
docker-compose -f config/docker-compose-local.yml up 