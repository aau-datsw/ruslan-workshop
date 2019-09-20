#!/bin/bash
docker-compose -f config/docker-compose-local.yml down
if [ -d "./web/ruslan-app/dist" ]
then 
    echo "Building the Angular 7 app..."
    cd ./web/ruslan-app 
    ng build --prod
    cd ..
    cd ..
fi

docker-compose -f config/docker-compose-local.yml build
docker-compose -f config/docker-compose-local.yml up 