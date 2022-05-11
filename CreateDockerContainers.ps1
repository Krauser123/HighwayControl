# Get redis for docker
docker pull redis
docker run --name redis-in-docker -d redis

# Stop containers
docker container stop highway-central-control-api
docker container stop highway-control-point-1
docker container stop highway-control-point-2
docker container stop highway-control-point-3

# Destroy containers
docker container rm highway-central-control-api
docker container rm highway-control-point-1
docker container rm highway-control-point-2
docker container rm highway-control-point-3

# Clean images
docker image rm highway-control-point
docker image rm highway-central-control-api

# Build docker file to create a image from control point
docker build -t highway-central-control-api -f ".\HighwayCentralControl_API\Dockerfile" .

# Build docker file to create a image from control point
docker build -t highway-control-point -f ".\HighwayControlPoint\Dockerfile" .

# Create container for Control API
docker run -d -p 8000:8000 --name highway-central-control-api highway-central-control-api

# Create multiple containers for Control Points
docker run -d -p 8001:8001 --name highway-control-point-1 highway-control-point
docker run -d -p 8002:8002 --name highway-control-point-2 highway-control-point
docker run -d -p 8003:8003 --name highway-control-point-3 highway-control-point