#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RealTimeData.API/RealTimeData.API.csproj", "RealTimeData.API/"]
RUN dotnet restore "RealTimeData.API/RealTimeData.API.csproj"
COPY . .
WORKDIR "/src/RealTimeData.API"
RUN dotnet build "RealTimeData.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealTimeData.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeData.API.dll"]