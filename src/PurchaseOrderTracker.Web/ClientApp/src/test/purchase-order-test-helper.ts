import { InquiryResult, ResultPurchaseOrder } from '../app/site/purchase-order/inquiry/inquiry.service';
import { EditQueryResult } from '../app/site/purchase-order/edit/edit.service';
import { SuppliersResult, SuppliersResultSupplier } from '../app/site/purchase-order/create/create.service';
import {
  EditLineItemsResult,
  EditLineItemsResultItem
} from '../app/site/purchase-order/edit-line-items/edit-line-items.service';

export class PurchaseOrderTestHelper {
  public static buildInquiryResult(items = [], pageCount = 0, pageNumber = 0, pageSize = 0): InquiryResult {
    return {
      pagedList: {
        items,
        pageCount,
        pageNumber,
        pageSize,
        hasPreviousPage: false,
        hasNextPage: false
      }
    };
  }

  public static buildInquiryResultWithItemsCount(count: number): InquiryResult {
    const items: ResultPurchaseOrder[] = [];

    for (let i = 1; i <= count; i++) {
      items[i - 1] = {
        id: i.toString(),
        createdDate: new Date(),
        orderNo: i.toString(),
        status: 'status',
        supplierName: 'supplierName' + i
      };
    }

    return PurchaseOrderTestHelper.buildInquiryResult(items, 1, 1, items.length);
  }

  public static buildSuppliersResult(): SuppliersResult {
    const suppliers: SuppliersResultSupplier[] = [];
    return {
      suppliers
    };
  }

  public static buildEditQueryResult(shipmentOptions = []): EditQueryResult {
    return {
      id: 1,
      createdDate: new Date(),
      supplierId: 1,
      shipmentId: 1,
      shipmentTrackingId: 1,
      currentState: 'currentState',
      orderNo: 'orderNo',
      isDelivered: false,
      isCancelled: false,
      isApproved: false,
      canTransitionToPendingApproval: false,
      canTransitionToApproved: false,
      canTransitionToCancelled: false,
      canShipmentBeUpdated: false,
      supplierOptions: [],
      shipmentOptions
    };
  }

  public static buildEditLineItemsResult(lineItems = []): EditLineItemsResult {
    return {
      purchaseOrderId: 1,
      purchaseOrderOrderNo: 'purchaseOrderOrderNo',
      lineItems,
      productOptions: []
    };
  }

  public static buildEditLineItemsResultItemWithCount(count: number): EditLineItemsResultItem[] {
    const items: EditLineItemsResultItem[] = [];

    for (let i = 1; i <= count; i++) {
      items[i - 1] = {
        id: i,
        productId: i,
        purchasePrice: 1,
        purchaseQty: 1
      };
    }

    return items;
  }
}
