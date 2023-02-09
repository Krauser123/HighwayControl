#Params
$networkName="ntHighwayControl"
$apiDockerFileLocation = ".\HighwayCentralControl_API\Dockerfile"
$pointDockerFileLocation = ".\HighwayControlPoint\Dockerfile"
$pointARDContainerName = "highway-control-point-1"
$pointOMEContainerName = "highway-control-point-2"
$pointUINContainerName = "highway-control-point-3"
$redisContainerName = "redis-in-docker"
$apiContainerName = "highway-central-control-api"

Write-Host "`nStopping containers"
docker container stop $redisContainerName
docker container stop $apiContainerName
docker container stop $pointARDContainerName
docker container stop $pointOMEContainerName
docker container stop $pointUINContainerName

Write-Host "`nDestroying containers"
docker container rm $redisContainerName
docker container rm $apiContainerName
docker container rm $pointARDContainerName
docker container rm $pointOMEContainerName
docker container rm $pointUINContainerName

Write-Host "`nClean images"
docker image rm highway-control-point-ard
docker image rm highway-control-point-ome
docker image rm highway-control-point-uin
docker image rm highway-central-control-api

Write-Host "`nClean network"
docker network rm $networkName

Write-Host "*****************************"
Write-Host "        Start Creation       "
Write-Host "*****************************"

Write-Host "`nCreate network"
docker network create $networkName

Write-Host "`nGet and run REDIS for docker"
docker pull redis
docker run -d -p 6379:6379 --network $networkName --name $redisContainerName redis

Write-Host "`nBuild docker file to create a image from control point"
docker build -t highway-central-control-api -f $apiDockerFileLocation .

Write-Host "`nBuild docker file to create a image from control point"
docker build -t highway-control-point-ard -f $pointDockerFileLocation"-ARD" .
docker build -t highway-control-point-ome -f $pointDockerFileLocation"-OME" .
docker build -t highway-control-point-uin -f $pointDockerFileLocation"-UIN" .

Write-Host "`nCreate container for Control API"
docker run -d -p 7999:80 -p 8000:443 --network $networkName --name $apiContainerName highway-central-control-api

Write-Host "`nCreate multiple containers for Control Points"
docker run -d -p 8001:8001 --network $networkName --name $pointARDContainerName highway-control-point-ard
docker run -d -p 8002:8002 --network $networkName --name $pointOMEContainerName highway-control-point-ome
docker run -d -p 8003:8003 --network $networkName --name $pointUINContainerName highway-control-point-uin