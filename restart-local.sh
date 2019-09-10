#!/bin/bash
docker-compose down
docker-compose -f docker-compose-local.yml build
docker-compose -f docker-compose-local.yml up 