FROM microsoft/dotnet:2.1-sdk AS build

# setup node
# https://github.com/aspnet/Announcements/issues/298 (Migrating from aspnetcore docker repos to dotnet)
# https://github.com/dotnet/dotnet-docker/issues/360 (Please support PowerShell core on SDK images)
WORKDIR /nodejs
ENV NODE_VERSION 10.15.0
ENV NODE_DOWNLOAD_SHA f0b4ff9a74cbc0106bbf3ee7715f970101ac5b1bbe814404d7a0673d1da9f674
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

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
# AS runtime
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build /app/src/PurchaseOrderTracker.WebApi/out ./WebApi
COPY --from=build /app/src/PurchaseOrderTracker.WebUI.Angular/out ./WebUI
RUN mv ./WebUI/ClientApp/dist/purchase-order-tracker/* ./WebUI/wwwroot