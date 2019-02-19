import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { MessagesService } from '../../shared/messages/messages.service';
import { DeleteService, DeleteCommand } from './delete.service';
import { EditService, EditQuery, EditQueryResult, EditCommand } from './edit.service';
import { idParam, suppliersUrl } from '../../config/routing.config';

@Component({
    templateUrl: './edit.component.html'
})
export class EditComponent implements OnInit {
    supplierId: number;
    model: EditViewModel;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private deleteService: DeleteService,
        private editService: EditService,
        private messagesService: MessagesService) {
    }

    ngOnInit(): void {
        this.supplierId = this.route.snapshot.params[idParam];
        this.refreshData();
    }

    refreshData(): void {
        const query = new EditQuery(this.supplierId);
        this.editService.handleQuery(query).subscribe(
            result => this.model = this.convertEditQueryResultToViewModel(result),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private convertEditQueryResultToViewModel(result: EditQueryResult): EditViewModel {
        // properties are exactly the same
        return <EditViewModel>result.suppliers[0];
    }

    onSubmit() {
        const command = new EditCommand(this.supplierId, this.model.name);
        this.editService.handleCommand(command).subscribe(
            () => this.messagesService.addMessage('Supplier updated'),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    onDelete() {
        const command = new DeleteCommand(this.supplierId);
        this.deleteService.handle(command).subscribe(
            () => this.router.navigateByUrl(suppliersUrl),
            err => this.messagesService.addHttpResponseError(err)
        );
    }
}

export class EditViewModel {
    id: number;
    name: string;
}
