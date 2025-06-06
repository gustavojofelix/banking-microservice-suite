# Dockerfile at services/AccountService/AccountService.API/Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects from root
COPY services/AccountService/AccountService.sln .
COPY services/AccountService/AccountService.Domain/ ./AccountService.Domain/
COPY services/AccountService/AccountService.Application/ ./AccountService.Application/
COPY services/AccountService/AccountService.Infrastructure/ ./AccountService.Infrastructure/
COPY services/AccountService/AccountService.API/ ./AccountService.API/

# Build
WORKDIR /src/AccountService.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AccountService.API.dll"]
