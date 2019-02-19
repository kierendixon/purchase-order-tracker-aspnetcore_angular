import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { MessagesService } from '../../shared/messages/messages.service';
import { InquiryService, InquiryQuery, InquiryResult } from './inquiry.service';
import { queryTypeQueryParam, pageNumberQueryParam } from '../../config/routing.config';

@Component({
    templateUrl: './inquiry.component.html',
    styleUrls: ['./inquiry.component.scss']
})
export class InquiryComponent implements OnInit {
    readonly defaultPageNumber = 1;
    model: InquiryResult;

    constructor(private route: ActivatedRoute,
        private inquiryService: InquiryService,
        private messagesService: MessagesService) {
    }

    ngOnInit(): void {
        this.route.queryParams.subscribe(params => {
            const queryType = params[queryTypeQueryParam];
            const pageNumber = params[pageNumberQueryParam] === undefined
                ? this.defaultPageNumber
                : params[pageNumberQueryParam];
            this.refreshData(queryType, pageNumber);
        });
    }

    refreshData(queryType: string, pageNumber: number): void {
        const query = new InquiryQuery(queryType, pageNumber);
        this.inquiryService.handle(query).subscribe(
            resp => this.model = resp,
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    hasRecords(): boolean {
        return this.model === undefined
            ? false
            : this.model.pagedList.items.length > 0;
    }
}
