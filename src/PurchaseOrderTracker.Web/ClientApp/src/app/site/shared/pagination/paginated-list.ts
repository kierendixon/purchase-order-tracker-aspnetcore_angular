export class PaginatedList<T> {
    constructor(readonly items: T[],
        readonly pageCount: number,
        readonly pageNumber: number,
        readonly pageSize: number,
        readonly hasPreviousPage: boolean,
        readonly hasNextPage: boolean) {
    }
}
