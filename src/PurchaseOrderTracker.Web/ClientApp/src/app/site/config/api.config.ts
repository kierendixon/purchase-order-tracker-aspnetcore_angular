import * as _ from 'lodash';

const baseApiUrl = '/api';

// main-site

export const baseReportingUrl = `${baseApiUrl}/reporting`;
export const reportingShipmentSummaryUrl = `${baseReportingUrl}/shipmentssummary`;

// shipment

export const baseShipmentUrl = `${baseApiUrl}/shipment`;

export function shipmentInquiryUrl(queryType: string, pageNumber: string): string {
    const compiled = _.template(baseShipmentUrl + '/inquiry?queryType=${queryType}&pageNumber=${pageNumber}');
    return compiled({ queryType: queryType, pageNumber: pageNumber });
}

export function shipmentUrl(shipmentId: number): string {
    const compiled = _.template(baseShipmentUrl + '/${shipmentId}');
    return compiled({ shipmentId: shipmentId });
}

export function shipmentStatusUrl(shipmentId: number): string {
    const compiled = _.template(baseShipmentUrl + '/${shipmentId}/status');
    return compiled({ shipmentId: shipmentId });
}

// supplier

export const baseSupplierUrl = `${baseApiUrl}/supplier`;

export function supplierInquiryUrl(queryType: string, pageNumber: string): string {
    const compiled = _.template(baseSupplierUrl + '/inquiry?queryType=${queryType}&pageNumber=${pageNumber}');
    return compiled({ queryType: queryType, pageNumber: pageNumber });
}

export function supplierUrl(supplierId: number): string {
    const compiled = _.template(baseSupplierUrl + '/${supplierId}');
    return compiled({ supplierId: supplierId });
}

export function supplierProductsUrl(supplierId: number, pageNumber?: string, productCodeFilter?: string): string {
    let template = baseSupplierUrl + '/${supplierId}/products';
    if (pageNumber) {
        template += '?pageNumber=${pageNumber}';
    }
    if (productCodeFilter) {
        if (pageNumber) {
            template += '&';
        }
        template += 'productCodeFilter=${productCodeFilter}';
    }

    const compiled = _.template(template);
    return compiled({ supplierId: supplierId, pageNumber: pageNumber, productCodeFilter: productCodeFilter });
}

export function supplierProductUrl(supplierId: number, productId: number): string {
    const compiled = _.template(baseSupplierUrl + '/${supplierId}/products/${productId}');
    return compiled({ supplierId: supplierId, productId: productId });
}

export function supplierProductCategoriesUrl(supplierId: number, pageNumber?: string): string {
    let template = baseSupplierUrl + '/${supplierId}/product-categories';
    if (pageNumber) {
        template += '?pageNumber=${pageNumber}';
    }

    const compiled = _.template(template);
    return compiled({ supplierId: supplierId, pageNumber: pageNumber });
}

export function supplierProductCategoryUrl(supplierId: number, categoryId: number): string {
    const compiled = _.template(baseSupplierUrl + '/${supplierId}/product-categories/${categoryId}');
    return compiled({ supplierId: supplierId, categoryId: categoryId });
}

// purchase order

export const basePurchaseOrderUrl = `${baseApiUrl}/purchaseorder`;

export function purchaseOrderInquiryUrl(queryType: string, pageNumber: string): string {
    const compiled = _.template(basePurchaseOrderUrl + '/inquiry?queryType=${queryType}&pageNumber=${pageNumber}');
    return compiled({ queryType: queryType, pageNumber: pageNumber });
}

export function purchaseOrderUrl(purchaseOrderId: number): string {
    const compiled = _.template(basePurchaseOrderUrl + '/${purchaseOrderId}');
    return compiled({ purchaseOrderId: purchaseOrderId });
}

export function purchaseOrderStatusUrl(purchaseOrderId: number): string {
    const compiled = _.template(basePurchaseOrderUrl + '/${purchaseOrderId}/status');
    return compiled({ purchaseOrderId: purchaseOrderId });
}

export function purchaseOrderLineItemUrl(purchaseOrderId: number, lineItemId: number): string {
    const compiled = _.template(basePurchaseOrderUrl + '/${purchaseOrderId}/line-items/${lineItemId}');
    return compiled({ purchaseOrderId: purchaseOrderId, lineItemId: lineItemId });
}

export function purchaseOrderLineItemsUrl(purchaseOrderId: number): string {
    const compiled = _.template(basePurchaseOrderUrl + '/${purchaseOrderId}/line-items');
    return compiled({ purchaseOrderId: purchaseOrderId });
}
