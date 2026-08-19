# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["KnowledgeHub.API/KnowledgeHub.API.csproj", "KnowledgeHub.API/"]
COPY ["KnowledgeHub.Application/KnowledgeHub.Application.csproj", "KnowledgeHub.Application/"]
COPY ["KnowledgeHub.Domain/KnowledgeHub.Domain.csproj", "KnowledgeHub.Domain/"]
COPY ["KnowledgeHub.Infrastructure/KnowledgeHub.Infrastructure.csproj", "KnowledgeHub.Infrastructure/"]

RUN dotnet restore "KnowledgeHub.API/KnowledgeHub.API.csproj"

COPY . .

WORKDIR "/src/KnowledgeHub.API"

RUN dotnet build "KnowledgeHub.API.csproj" -c Release -o /app/build

RUN dotnet publish "KnowledgeHub.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "KnowledgeHub.API.dll"]