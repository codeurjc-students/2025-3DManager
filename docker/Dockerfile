# BUILD

# Use .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project solution
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.sln ./ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.Server/3DMANAGER-APP.Server.csproj ./3DMANAGER-APP.Server/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.BLL/3DMANAGER-APP.BLL.csproj ./3DMANAGER-APP.BLL/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.DAL/3DMANAGER-APP.DAL.csproj ./3DMANAGER-APP.DAL/ 
COPY ../Code/3DMANAGER-APP/3dmanager-app.client/3dmanager-app.client.esproj ./3dmanager-app.client/

# Restore dependencies
RUN dotnet restore 3DMANAGER-APP.sln

COPY ../Code/3DMANAGER-APP/ ./

# Build React frontend
WORKDIR /src/3dmanager-app.client 
RUN npm install 
RUN npm run build

# Publish backend
WORKDIR /src/3DMANAGER-APP.Server 
RUN dotnet publish -c Release -o /app/publish

# Serve frontend static files from the backend
RUN mkdir -p /app/publish/wwwroot 
RUN cp -r /src/3dmanager-app.client/build/* /app/publish/wwwroot/

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app

# Expose HTTPS
EXPOSE 443

# Environment variables
ENV ASPNETCORE_URLS=https://+:443 
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish ./

# Health check
HEALTHCHECK --interval=10s --timeout=3s --retries=5 \
 CMD curl -k https://localhost/health || exit 1

 # Container entrypoint
ENTRYPOINT ["dotnet", "3DMANAGER-APP.Server.dll"]