# Build docker file to create a image
#docker build -t highway-control-point -f ".\HighwayControlPoint\Dockerfile" .

# Create multiple containers
docker run -d -p 8001:8001 --name highway-control-point-1 highway-control-point
docker run -d -p 8002:8002 --name highway-control-point-2 highway-control-point
docker run -d -p 8003:8003 --name highway-control-point-3 highway-control-point