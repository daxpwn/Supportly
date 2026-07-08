export interface PagedResponse<T> {
  totalCount: number;
  pagesCount: number;
  items: T[];
  currentPage: number;
  perPage: number;
}
