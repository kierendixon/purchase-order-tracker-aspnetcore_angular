FROM mcr.microsoft.com/dotnet/sdk:6.0.301 AS build

# setup node
# https://github.com/aspnet/Announcements/issues/298 (Migrating from aspnetcore docker repos to dotnet)
# https://github.com/dotnet/dotnet-docker/issues/360 (Please support PowerShell core on SDK images)
WORKDIR /nodejs
ENV NODE_VERSION 16.14.2
ENV NODE_DOWNLOAD_SHA 57e02c27eb5e52f560f72d96240e898cb52818dc9fc50f45478ce39ece38583a
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

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

# build project
WORKDIR /app/src/PurchaseOrderTracker.WebUI.Admin
RUN dotnet publish --no-restore -c Release -o /out

# build runtime container
# AS runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0.3
WORKDIR /app
COPY --from=build /out .


# TODO: run frontend tests