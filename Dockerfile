# escape=`

FROM microsoft/dotnet:2.1-sdk AS build

# setup node
# https://github.com/aspnet/Announcements/issues/298 (Migrating from aspnetcore docker repos to dotnet)
# https://github.com/dotnet/dotnet-docker/issues/360 (Please support PowerShell core on SDK images)
WORKDIR /nodejs
ENV NODE_VERSION 8.9.4
ENV NODE_DOWNLOAD_SHA 02e3c65000ac055e05c604aec4cf318212efbd4b60a945ed319072d58314ca32
RUN curl https://nodejs.org/dist/v%NODE_VERSION%/node-v%NODE_VERSION%-win-x64.zip -o nodejs.zip &&`
    tar -xf nodejs.zip --strip-components=1 &&`
	del nodejs.zip &&`
    setx PATH "%PATH%;C:\nodejs"

# copy csproj and restore as distinct layers
WORKDIR /app
COPY *.sln . 
COPY src/PurchaseOrderTracker.DAL/*.csproj ./src/PurchaseOrderTracker.DAL/ 
COPY src/PurchaseOrderTracker.Domain/*.csproj  ./src/PurchaseOrderTracker.Domain/ 
COPY src/PurchaseOrderTracker.Web/*.csproj  ./src/PurchaseOrderTracker.Web/ 
COPY test/PurchaseOrderTracker.Domain.Tests/*.csproj ./test/PurchaseOrderTracker.Domain.Tests/ 
COPY test/PurchaseOrderTracker.Web.Tests/*.csproj ./test/PurchaseOrderTracker.Web.Tests/
RUN dotnet restore

# copy everything else and build app
COPY src/PurchaseOrderTracker.DAL/. ./src/PurchaseOrderTracker.DAL/ 
COPY src/PurchaseOrderTracker.Domain/.  ./src/PurchaseOrderTracker.Domain/ 
COPY src/PurchaseOrderTracker.Web/.  ./src/PurchaseOrderTracker.Web/ 
COPY test/PurchaseOrderTracker.Domain.Tests/. ./test/PurchaseOrderTracker.Domain.Tests/ 
COPY test/PurchaseOrderTracker.Web.Tests/. ./test/PurchaseOrderTracker.Web.Tests/
RUN dotnet publish -c Debug -o out

# build runtime container
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /nodejs
ENV NODE_VERSION 8.9.4
ENV NODE_DOWNLOAD_SHA 02e3c65000ac055e05c604aec4cf318212efbd4b60a945ed319072d58314ca32
RUN curl https://nodejs.org/dist/v%NODE_VERSION%/node-v%NODE_VERSION%-win-x64.zip -o nodejs.zip &&`
    tar -xf nodejs.zip --strip-components=1 &&`
	del nodejs.zip &&`
    setx PATH "%PATH%;C:\nodejs"
WORKDIR /app
COPY --from=build /app/src/PurchaseOrderTracker.Web/out ./
ENTRYPOINT ["dotnet", "PurchaseOrderTracker.Web.dll"]
EXPOSE 80