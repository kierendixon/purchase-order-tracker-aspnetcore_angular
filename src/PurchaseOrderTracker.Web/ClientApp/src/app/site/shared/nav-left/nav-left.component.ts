import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { purchaseOrdersUrl, shipmentsUrl, suppliersUrl } from '../../config/routing.config';

@Component({
  selector: 'nav-left',
  templateUrl: './nav-left.component.html',
  styleUrls: ['./nav-left.component.scss']
})
export class NavLeftComponent {
  // TODO: use activeROute instead of router ?
  constructor(private activeRoute: ActivatedRoute, private router: Router) {}

  public isDisplayPurchaseOrdersNav(): boolean {
    return this.router.url.toLowerCase().indexOf(purchaseOrdersUrl) === 0;
  }

  public isDisplaySuppliersNav(): boolean {
    return this.router.url.toLowerCase().indexOf(suppliersUrl) === 0;
  }

  public isDisplayShipmentsNav(): boolean {
    return this.router.url.toLowerCase().indexOf(shipmentsUrl) === 0;
  }
}
