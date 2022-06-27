# build backend project
#######################
FROM mcr.microsoft.com/dotnet/sdk:6.0.301 AS dotnet-build

# copy files which don't change often and then 'dotnet restore' to cache as image layers
# keep this the same between all Dockerfiles to maximise cache reuse
WORKDIR /app
COPY PurchaseOrderTracker.sln ./ 
COPY src/PurchaseOrderTracker.Application/*.csproj ./src/PurchaseOrderTracker.Application/
COPY src/PurchaseOrderTracker.AspNet.Common/*.csproj ./src/PurchaseOrderTracker.AspNet.Common/
COPY src/PurchaseOrderTracker.Cache/*.csproj ./src/PurchaseOrderTracker.Cache/
COPY src/PurchaseOrderTracker.Domain/*.csproj  ./src/PurchaseOrderTracker.Domain/
COPY src/PurchaseOrderTracker.Identity/*.csproj  ./src/PurchaseOrderTracker.Identity/
COPY src/PurchaseOrderTracker.Persistence/*.csproj  ./src/PurchaseOrderTracker.Persistence/
COPY src/PurchaseOrderTracker.WebApi/*.csproj  ./src/PurchaseOrderTracker.WebApi/
COPY src/PurchaseOrderTracker.WebUI.Admin/*.csproj  ./src/PurchaseOrderTracker.WebUI.Admin/
COPY src/PurchaseOrderTracker.WebUI.Angular/*.csproj  ./src/PurchaseOrderTracker.WebUI.Angular/
COPY test/PurchaseOrderTracker.Domain.Tests/*.csproj ./test/PurchaseOrderTracker.Domain.Tests/
COPY test/PurchaseOrderTracker.Application.Tests/*.csproj ./test/PurchaseOrderTracker.Application.Tests/
RUN dotnet restore

# copy everything else
COPY . .

# build
WORKDIR /app/src/PurchaseOrderTracker.WebUI.Admin
RUN dotnet publish --no-restore -c Release -o /out


# build frontend project
########################
FROM node:16.14.2 AS node-build
WORKDIR /app
COPY src/PurchaseOrderTracker.WebUI.Admin/ClientApp  .
RUN npm install
ENV BUILD_PATH=/out
RUN npm run build


# build runtime container
#########################
FROM mcr.microsoft.com/dotnet/aspnet:6.0.3
WORKDIR /app
COPY --from=dotnet-build /out .
COPY --from=node-build /out ./wwwroot
