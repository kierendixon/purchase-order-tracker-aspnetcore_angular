import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date-struct';

import { EditService, EditQuery, EditCommand, EditQueryResult } from './edit.service';
import { EditStatusService, EditStatusCommand } from './edit-status.service';
import { DeleteService, DeleteCommand } from './delete.service';
import { MessagesService } from '../../shared/messages/messages.service';
import { idParam, shipmentsUrl } from '../../config/routing.config';

@Component({
    templateUrl: './edit.component.html'
})
export class EditComponent implements OnInit {
    shipmentId: number;
    model: EditViewModel;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private messagesService: MessagesService,
        private editService: EditService,
        private editStatusService: EditStatusService,
        private deleteService: DeleteService) {
    }

    ngOnInit(): void {
        this.shipmentId = this.route.snapshot.params[idParam];
        this.refreshData();
    }

    private refreshData(): void {
        const query = new EditQuery(this.shipmentId);
        this.editService.handleQuery(query).subscribe(
            result => this.model = this.convertEditQueryResultToViewModel(result),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private convertEditQueryResultToViewModel(result: EditQueryResult): EditViewModel {
        const editViewModel = new EditViewModel();
        Object.keys(result).forEach(k => {
            if (k === EditViewModel.estimatedArrivalDatePropertyName && result[k]) {
                const eta = new Date(result[k]);
                editViewModel.estimatedArrivalDate = { day: eta.getUTCDay(), month: eta.getUTCMonth(), year: eta.getUTCFullYear() };
            } else {
                editViewModel[k] = result[k];
            }
        });
        return editViewModel;
    }

    onSubmit() {
        const editCommand = this.buildEditCommand();
        this.editService.handleCommand(editCommand).subscribe(
            () => this.messagesService.addMessage('Update successful'),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private buildEditCommand(): EditCommand {
        return new EditCommand(this.model.id,
            this.model.trackingId,
            this.model.company,
            new Date(this.model.estimatedArrivalDate.year, this.model.estimatedArrivalDate.month, this.model.estimatedArrivalDate.day),
            this.model.comments,
            this.model.shippingCost,
            this.model.destinationAddress
        );
    }

    onDelete() {
        const deleteCommand = new DeleteCommand(this.model.id);
        this.deleteService.handle(deleteCommand).subscribe(
            () => this.router.navigateByUrl(shipmentsUrl),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    onUpdateStatus(status: string) {
        const editCommand = new EditStatusCommand(this.model.id, status);
        this.editStatusService.handle(editCommand).subscribe(
            () => {
                this.messagesService.addMessage('Status updated');
                this.refreshData();
            },
            err => this.messagesService.addHttpResponseError(err)
        );
    }
}

class EditViewModel {
    static estimatedArrivalDatePropertyName = 'estimatedArrivalDate';

    id: number;
    trackingId: string;
    company: string;
    comments: string;
    shippingCost: number;
    destinationAddress: string;
    isDelivered: boolean;
    canTransitionToAwaitingShipping: boolean;
    canTransitionToShipped: boolean;
    canTransitionToDelivered: boolean;
    estimatedArrivalDate: NgbDateStruct;
}
