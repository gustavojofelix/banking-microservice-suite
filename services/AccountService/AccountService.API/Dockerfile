# Dockerfile at services/AccountService/AccountService.API/Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects from root
COPY services/AccountService/AccountService.sln .
COPY services/AccountService/AccountService.Domain/ ./AccountService.Domain/
COPY services/AccountService/AccountService.Application/ ./AccountService.Application/
COPY services/AccountService/AccountService.Infrastructure/ ./AccountService.Infrastructure/
COPY services/AccountService/AccountService.API/ ./AccountService.API/
# Copy entrypoint script from source (relative to context)
COPY services/AccountService/AccountService.API/entrypoint.sh .

# Build
WORKDIR /src/AccountService.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copy the entrypoint script from the build context (not from publish)
COPY services/AccountService/AccountService.API/entrypoint.sh .

# Make it executable
RUN chmod +x ./entrypoint.sh

# Install netcat for DB wait script
RUN apt-get update && apt-get install -y netcat-openbsd

#ENTRYPOINT ["dotnet", "AccountService.API.dll"]
# Change entrypoint
ENTRYPOINT ["./entrypoint.sh", "account-postgres", "dotnet", "AccountService.API.dll"]
