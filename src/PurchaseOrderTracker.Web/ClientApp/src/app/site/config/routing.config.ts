import * as _ from 'lodash';
import { mainSiteUrl } from '../../config/routing.config';

// query parameters

export const queryTypeQueryParam = 'queryType';
export const pageNumberQueryParam = 'pageNumber';
export const idParam = 'id';

// supplier URLs

export const suppliersUrl = `${mainSiteUrl}/suppliers`;

export function editSupplierUrl(supplierId: number): string {
    const compiled = _.template(suppliersUrl + '/${supplierId}');
    return compiled({ supplierId: supplierId });
}

// shipment URLs

export const shipmentsUrl = `${mainSiteUrl}/shipments`;

export function editShipmentUrl(shipmentId: number): string {
    const compiled = _.template(shipmentsUrl + '/${shipmentId}');
    return compiled({ shipmentId: shipmentId });
}

// purchase-order URLs

export const purchaseOrdersUrl = `${mainSiteUrl}/purchase-orders`;

export function editPurchaseOrderUrl(purchaseOrderId: number): string {
    const compiled = _.template(purchaseOrdersUrl + '/${purchaseOrderId}');
    return compiled({ purchaseOrderId: purchaseOrderId });
}

// resolver guard data keys

export const shipmentSummaryResultResolverDataKey = 'ShipmentSummaryResult';

