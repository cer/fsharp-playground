#! /bin/bash -e

./start-db.sh

docker-compose up -d --build

./wait-for-services.sh

curl --fail localhost:8081/customer

export CUSTOMER_ID=$(curl --fail -X POST -H 'content-type:application/json' --data '{ "Name": "Chris"}' localhost:8081/customer | jq .id)

curl --fail localhost:8081/customer/$CUSTOMER_ID
