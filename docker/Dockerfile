# BUILD
# Frontend build
FROM node:20 AS frontend-build
WORKDIR /app 

COPY ../Code/3DMANAGER-APP/3dmanager-app.client/package*.json ./ 
RUN npm install 

COPY ../Code/3DMANAGER-APP/3dmanager-app.client/ ./ 
RUN npm run build

# Backend build with .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project solution
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.sln ./ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.Server/3DMANAGER-APP.Server.csproj ./3DMANAGER-APP.Server/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.BLL/3DMANAGER-APP.BLL.csproj ./3DMANAGER-APP.BLL/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.DAL/3DMANAGER-APP.DAL.csproj ./3DMANAGER-APP.DAL/ 
COPY ../Code/3DMANAGER-APP/3dmanager-app.client/3dmanager-app.client.esproj ./3dmanager-app.client/
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.TEST/3DMANAGER-APP.TEST.csproj ./3DMANAGER-APP.TEST/ 
COPY ../Code/3DMANAGER-APP/TestingSupport/3DMANAGER-APP.TestingSupport.csproj ./TestingSupport/ 
# Restore dependencies
RUN dotnet restore 3DMANAGER-APP.sln

COPY ../Code/3DMANAGER-APP/ ./

# Publish backend
WORKDIR /src/3DMANAGER-APP.Server 
RUN dotnet publish -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app

# Expose HTTPS
EXPOSE 443

# Environment variables
ENV ASPNETCORE_URLS=https://+:443 
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish ./

RUN mkdir -p /app/wwwroot 
COPY --from=frontend-build /app/dist /app/wwwroot

# Health check
HEALTHCHECK --interval=10s --timeout=3s --retries=5 \
 CMD curl -k https://localhost/health || exit 1

 # Container entrypoint
ENTRYPOINT ["dotnet", "3DMANAGER-APP.Server.dll"]