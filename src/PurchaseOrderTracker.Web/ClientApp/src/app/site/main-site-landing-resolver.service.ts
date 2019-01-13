import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


import { MainSiteService, ShipmentSummaryResult } from './main-site.service';
import { MessagesService } from './shared/messages/messages.service';

// TODO: add resolvers to all pages to prefetch data or use another mechanism such as displaying a spinner?
@Injectable()
export class MainSiteLandingResolver implements Resolve<ShipmentSummaryResult> {
    constructor(private mainSiteService: MainSiteService,
        private router: Router,
        private messagesService: MessagesService) {
    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ShipmentSummaryResult> {
        return this.mainSiteService.handleShipmentSummaryQuery().pipe(
            map(
                resp => {
                    return resp;
                },
                err => {
                    this.messagesService.addHttpResponseError(err);
                    return null;
                }
            )
        );
    }
}
