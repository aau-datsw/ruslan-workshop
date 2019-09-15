#!/bin/bash
docker-compose down
if [ ! -d "./web/ruslan-app/dist"]
then 
    echo "Building the Angular 7 app..."
    pushd ./web/ruslan-app 
    ng build --prod
    popd
fi
docker-compose -f docker-compose-local.yml build
docker-compose -f docker-compose-local.yml up 