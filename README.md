# HighwayControl
###### .NET, React, Microservices, Docker, Redis

### History

> This project started as an exercise related to a C# API with Swagger.
>   \
> A frontend was added to it in React.
>   \
> Redis was added later.
>   \
> Microservices were added later.
>   \
> Support for Docker was added later.
>   \
> And I think I'm still working on it.

### Features
We can see data from several fake sensors on a highway that catch data about the vehicles.

We can see the catch data date, speed and car plates.
| sensorId | carSpeed | carPlate | dataDate |
| ------ | ------ | ------ | ------ |
| G5 | 97 | 5260 VAJ | 2022-09-02
| G10 | 121 | 1515 CID | 2022-09-02
| G15 | 125 | 3699 GBD | 2022-09-02

## How works
In Visual Studio we can launch and debug the projects but the interesting thing here is used Docker containers as microservices.

In this way, we also can add as many HighWaySensors (in fact a microservice in docker) as I want with few easy modifications.

## Installation
Create docker containers executing PS1 in root folder.
```
cd HighwayControl\
./CreateDockerContainers.ps1
```

Run frontend
```
cd HighwayControl\highwaycentralcontrol_front\
npm i
npm start
```