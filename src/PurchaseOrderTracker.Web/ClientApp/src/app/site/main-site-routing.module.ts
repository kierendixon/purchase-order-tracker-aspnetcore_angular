import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../infrastructure/security/auth-guard';
import { MainSiteComponent } from './main-site.component';
import { MainSiteLandingComponent } from './main-site-landing.component';
import { MainSiteLandingResolver } from './main-site-landing-resolver.service';

const mainSiteRoutes: Routes = [
    {
        path: 'main-site',
        component: MainSiteComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                component: MainSiteLandingComponent,
                resolve: { ShipmentSummaryResult: MainSiteLandingResolver }
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(mainSiteRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class MainSiteRoutingModule { }
