# escape=`

FROM microsoft/dotnet:2.1-sdk AS build

# setup node
# https://github.com/aspnet/Announcements/issues/298 (Migrating from aspnetcore docker repos to dotnet)
# https://github.com/dotnet/dotnet-docker/issues/360 (Please support PowerShell core on SDK images)
WORKDIR /nodejs
ENV NODE_VERSION 10.15.0
ENV NODE_DOWNLOAD_SHA c1dbc9372ad789cd21727cb5f63b4a44ed3eae216763959cff8e68e68c6fcfe1
RUN curl https://nodejs.org/dist/v%NODE_VERSION%/node-v%NODE_VERSION%-win-x64.zip -o nodejs.zip &&`
    tar -xf nodejs.zip --strip-components=1 &&`
    del nodejs.zip &&`
    setx PATH "%PATH%;C:\nodejs"

# copy files which don't change often and then 'dotnet restore' to cache as image layers
WORKDIR /app
COPY *.sln ./ 
COPY *.ruleset ./
COPY stylecop.json ./
COPY src/PurchaseOrderTracker.Application/*.csproj ./src/PurchaseOrderTracker.Application/ 
COPY src/PurchaseOrderTracker.Cache/*.csproj ./src/PurchaseOrderTracker.Cache/ 
COPY src/PurchaseOrderTracker.Domain/*.csproj  ./src/PurchaseOrderTracker.Domain/ 
COPY src/PurchaseOrderTracker.Persistence/*.csproj  ./src/PurchaseOrderTracker.Persistence/ 
COPY src/PurchaseOrderTracker.WebApi/*.csproj  ./src/PurchaseOrderTracker.WebApi/ 
COPY src/PurchaseOrderTracker.WebUI.Angular/*.csproj  ./src/PurchaseOrderTracker.WebUI.Angular/ 
COPY test/PurchaseOrderTracker.Domain.Tests/*.csproj ./test/PurchaseOrderTracker.Domain.Tests/ 
COPY test/PurchaseOrderTracker.Application.Tests/*.csproj ./test/PurchaseOrderTracker.Application.Tests/
RUN dotnet restore

# copy everything else and build app
COPY src/PurchaseOrderTracker.Application/. ./src/PurchaseOrderTracker.Application/ 
COPY src/PurchaseOrderTracker.Cache/. ./src/PurchaseOrderTracker.Cache/ 
COPY src/PurchaseOrderTracker.Domain/.  ./src/PurchaseOrderTracker.Domain/ 
COPY src/PurchaseOrderTracker.Persistence/.  ./src/PurchaseOrderTracker.Persistence/ 
COPY src/PurchaseOrderTracker.WebApi/.  ./src/PurchaseOrderTracker.WebApi/ 
COPY src/PurchaseOrderTracker.WebUI.Angular/.  ./src/PurchaseOrderTracker.WebUI.Angular/ 
COPY test/PurchaseOrderTracker.Domain.Tests/. ./test/PurchaseOrderTracker.Domain.Tests/ 
COPY test/PurchaseOrderTracker.Application.Tests/. ./test/PurchaseOrderTracker.Application.Tests/
RUN dotnet publish -c Release -o out

# build runtime container
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/src/PurchaseOrderTracker.WebApi/out ./
ENTRYPOINT ["dotnet", "PurchaseOrderTracker.Web.dll"]
EXPOSE 80