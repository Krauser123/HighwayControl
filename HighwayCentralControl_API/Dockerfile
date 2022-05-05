#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HighwayCentralControl_API/HighwayCentralControl_API.csproj", "HighwayCentralControl_API/"]
RUN dotnet restore "HighwayCentralControl_API/HighwayCentralControl_API.csproj"
COPY . .
WORKDIR "/src/HighwayCentralControl_API"
RUN dotnet build "HighwayCentralControl_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HighwayCentralControl_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HighwayCentralControl_API.dll"]