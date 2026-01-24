# BUILD
FROM node:20 AS frontend-build
WORKDIR /app 

COPY ../Code/3DMANAGER-APP/3dmanager-app.client/package*.json ./ 
RUN npm install 

COPY ../Code/3DMANAGER-APP/3dmanager-app.client/ ./ 
RUN npm run build

# Backend build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.sln ./ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.Server/3DMANAGER-APP.Server.csproj ./3DMANAGER-APP.Server/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.BLL/3DMANAGER-APP.BLL.csproj ./3DMANAGER-APP.BLL/ 
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.DAL/3DMANAGER-APP.DAL.csproj ./3DMANAGER-APP.DAL/ 
COPY ../Code/3DMANAGER-APP/3dmanager-app.client/3dmanager-app.client.esproj ./3dmanager-app.client/
COPY ../Code/3DMANAGER-APP/3DMANAGER-APP.TEST/3DMANAGER-APP.TEST.csproj ./3DMANAGER-APP.TEST/ 
COPY ../Code/3DMANAGER-APP/TestingSupport/3DMANAGER-APP.TestingSupport.csproj ./TestingSupport/ 

RUN dotnet restore 3DMANAGER-APP.sln
COPY ../Code/3DMANAGER-APP/ ./

WORKDIR /src/3DMANAGER-APP.Server 
RUN dotnet publish -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app

# Generate PFX certificate
RUN mkdir -p /https && \
    openssl req -x509 -newkey rsa:2048 -nodes \
        -keyout /https/key.pem \
        -out /https/cert.pem \
        -days 365 \
        -subj "/CN=localhost" && \
    openssl pkcs12 -export \
        -out /https/https.pfx \
        -inkey /https/key.pem \
        -in /https/cert.pem \
        -password pass:3dmanagerPASS1234

# Environment variables
ENV ASPNETCORE_URLS=https://+:443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/https.pfx

COPY --from=build /app/publish ./

RUN mkdir -p /app/wwwroot 
COPY --from=frontend-build /app/dist /app/wwwroot

# Health check
HEALTHCHECK --interval=10s --timeout=3s --retries=5 \
 CMD curl -k https://localhost/health || exit 1

EXPOSE 443

ENTRYPOINT ["dotnet", "3DMANAGER-APP.Server.dll"]
