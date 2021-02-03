#! /bin/bash -e

docker-compose up -d db

docker-compose run --rm wait-for-db

docker-compose run --rm dbsetup
