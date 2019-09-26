#!/bin/bash
docker-compose -f config/docker-compose-local.yml down
if [ ! -d "./web/ruslan-stonks/dist" ]
then 
    echo "Building the Angular 7 app..."
    cd ./web/ruslan-stonks
    ng build --prod
    cd ..
    cd ..
fi

python3 build.py

docker-compose -f config/docker-compose-local.yml build
docker-compose -f config/docker-compose-local.yml up 