# move to directory with .sln file
cd ./../../../

docker build --pull --rm -f "./Source/Monitoring.Api/Dockerfile" -t monitoring.api:tests-local "./"

docker save --output ./Source/Monitoring.Api/Shell/test_period_task_backend_api.tar monitoring.api:tests-local
