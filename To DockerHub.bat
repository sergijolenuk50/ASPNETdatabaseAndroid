@echo off

echo Docker login...
docker login

cd "WebCatApi"

echo Building Docker image api...
docker build -t pv212-asp-api .

echo Tagging Docker image api...
docker tag pv212-asp-api:latest novakvova/pv212-asp-api:latest

echo Pushing Docker image api to repository...
docker push novakvova/pv212-asp-api:latest

echo Done ---api---!
pause
