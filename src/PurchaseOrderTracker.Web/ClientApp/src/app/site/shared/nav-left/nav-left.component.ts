import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    // TODO: remove tslint ignore
    // tslint:disable-next-line:component-selector
    selector: 'nav-left',
    templateUrl: './nav-left.component.html',
    styleUrls: ['./nav-left.component.css']
})
export class NavLeftComponent {

    constructor(private activeRoute: ActivatedRoute, private router: Router) {
    }

    public isDisplayPurchaseOrdersNav(): boolean {
        return this.router.url.toLowerCase().indexOf('/purchase-orders') === 10;
    }

    public isDisplaySuppliersNav(): boolean {
        return this.router.url.toLowerCase().indexOf('/suppliers') === 10;
    }

    public isDisplayShipmentsNav(): boolean {
        return this.router.url.toLowerCase().indexOf('/shipments') === 10;
    }
}
