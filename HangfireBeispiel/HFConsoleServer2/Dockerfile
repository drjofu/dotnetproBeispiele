#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HFConsoleServer2/HFConsoleServer2.csproj", "HFConsoleServer2/"]
RUN dotnet restore "HFConsoleServer2/HFConsoleServer2.csproj"
COPY . .
WORKDIR "/src/HFConsoleServer2"
RUN dotnet build "HFConsoleServer2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HFConsoleServer2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HFConsoleServer2.dll"]