
# build all images
docker build --tag purchase-order-tracker-webapi -f src/PurchaseOrderTracker.WebApi/Dockerfile .
docker build --tag purchase-order-tracker-angular -f src/PurchaseOrderTracker.WebUI.Angular/Dockerfile .
docker build --tag purchase-order-tracker-admin -f src/PurchaseOrderTracker.WebUI.Admin/Dockerfile .
docker build --tag purchase-order-tracker-identity -f src/PurchaseOrderTracker.Identity/Dockerfile .

# start the application
docker-compose up