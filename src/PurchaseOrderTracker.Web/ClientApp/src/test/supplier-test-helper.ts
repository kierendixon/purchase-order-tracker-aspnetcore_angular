import { InquiryResult, ResultSupplier } from '../app/site/supplier/inquiry/inquiry.service';
import { EditQueryResultSupplier } from '../app/site/supplier/edit/edit.service';
import { EditViewModel } from '../app/site/supplier/edit/edit.component';
import { EditProductCategoriesResult } from '../app/site/supplier/edit-product-categories/edit-product-categories.service';
import { ProductCategory } from '../app/site/supplier/edit-product-categories/edit-product-categories.component';
import { PaginatedList } from '../app/site/shared/pagination/paginated-list';
import {
  EditProductsResult,
  EditProductsResultProduct
} from '../app/site/supplier/edit-products/edit-products.service';
import { EditProductViewModel } from '../app/site/supplier/edit-products/edit-products.component';

export class SupplierTestHelper {
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
    const items: ResultSupplier[] = [];

    for (let i = 0; i < count; i++) {
      items[i] = {
        id: i.toString(),
        name: 'name' + i
      };
    }

    return SupplierTestHelper.buildInquiryResult(items, 1, 1, items.length);
  }

  public static buildEditQueryResultSupplier(): EditQueryResultSupplier {
    return {
      id: 1,
      name: 'name'
    };
  }

  public static buildEditViewModel(): EditViewModel {
    return {
      id: 1,
      name: 'name'
    };
  }

  public static buildEditProductCategoriesResult(): EditProductCategoriesResult {
    return {
      supplierName: 'supplierName',
      categories: {
        items: [],
        pageCount: 0,
        pageNumber: 0,
        pageSize: 0,
        hasPreviousPage: false,
        hasNextPage: false
      }
    };
  }

  public static buildPaginatedListOfProductCategory(length: number): PaginatedList<ProductCategory> {
    const items: ProductCategory[] = [];
    for (let i = 0; i < length; i++) {
      const productCategory: ProductCategory = {
        id: i + 1,
        name: 'name' + (i + 1)
      };
      items.push(productCategory);
    }

    return new PaginatedList<ProductCategory>(items, 1, 1, items.length, false, false);
  }

  public static buildEditProductsResult(): EditProductsResult {
    const editProductsResultProduct: EditProductsResultProduct = {
      productId: 1,
      prodCode: 'prodCode1',
      name: 'name1',
      categoryId: 1,
      price: 1
    };

    return {
      products: {
        items: [editProductsResultProduct],
        pageCount: 1,
        pageNumber: 1,
        pageSize: 1,
        hasPreviousPage: false,
        hasNextPage: false
      },
      productsAreFiltered: false,
      categoryOptions: {},
      supplierId: 1,
      supplierName: 'name1'
    };
  }

  public static buildPaginatedListOfEditProductViewModel(length: number): PaginatedList<EditProductViewModel> {
    const items: EditProductViewModel[] = [];
    for (let i = 0; i < length; i++) {
      const editProductModel: EditProductViewModel = {
        productId: i + 1,
        prodCode: 'prodCode' + (i + 1),
        name: 'name' + (i + 1),
        categoryId: i + 1,
        price: i
      };
      items.push(editProductModel);
    }

    return new PaginatedList<EditProductViewModel>(items, 1, 1, items.length, false, false);
  }
}
