#!/bin/bash

# move to directory with .sln file
cd ./../../../

docker build --pull --rm -f "./Source/Monitoring.Api/Shell/Dockerfile" -t monitoring.api:202307111712 "./"
docker pull redocly/cli

docker run --name monitoring.api -p 8081:80 -d monitoring.api:202307111712
docker run -p 8080:80 -e SPEC_URL=http://localhost:8081/swagger/v1/swagger.json redocly/redoc