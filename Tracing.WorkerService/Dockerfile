#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tracing.WorkerService/Tracing.WorkerService.csproj", "Tracing.WorkerService/"]
RUN dotnet restore "Tracing.WorkerService/Tracing.WorkerService.csproj"
COPY . .
WORKDIR "/src/Tracing.WorkerService"
RUN dotnet build "Tracing.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracing.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracing.WorkerService.dll"]