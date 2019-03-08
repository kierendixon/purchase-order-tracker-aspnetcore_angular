import { EditViewModel } from '../app/site/shipment/edit/edit.component';
import { EditQueryResult } from '../app/site/shipment/edit/edit.service';
import { InquiryResult, ResultShipment } from '../app/site/shipment/inquiry/inquiry.service';

export class ShipmentTestHelper {
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
    const items: ResultShipment[] = [];

    for (let i = 1; i <= count; i++) {
      items[i - 1] = {
        id: i,
        trackingId: 'trackingId' + 1,
        company: 'company' + 1,
        estimatedArrivalDate: new Date(),
        comments: 'comments',
        shippingCost: 1,
        status: 'status',
        destinationAddress: 'destinationAddress',
        isDelayed: false,
        isDelayedMoreThan7Days: false,
        isScheduledForDeliveryToday: false
      };
    }

    return ShipmentTestHelper.buildInquiryResult(items, 1, 1, items.length);
  }

  public static buildEditQueryResult(): EditQueryResult {
    return {
      id: 1,
      trackingId: 'trackingId',
      company: 'company',
      comments: 'comments',
      estimatedArrivalDate: new Date().toString(),
      shippingCost: 1,
      destinationAddress: 'destinationAddress',
      isDelivered: false,
      canTransitionToAwaitingShipping: false,
      canTransitionToDelivered: false,
      canTransitionToShipped: false
    };
  }

  public static buildEditViewModel(): EditViewModel {
    return {
      id: 1,
      trackingId: 'trackingId',
      company: 'company',
      comments: 'comments',
      estimatedArrivalDate: { day: 1, month: 1, year: 2019 },
      shippingCost: 1,
      destinationAddress: 'destinationAddress',
      isDelivered: false,
      canTransitionToAwaitingShipping: false,
      canTransitionToDelivered: false,
      canTransitionToShipped: false
    };
  }
}
