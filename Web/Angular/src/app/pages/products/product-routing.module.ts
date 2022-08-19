import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreatePolicyComponent } from './create-policy/create-policy.component';
import { PolicyDetailComponent } from './policy-detail/policy-detail.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductComponent } from './product.component';


const routes: Routes = [{
  path: '',
  component: ProductComponent,
  children: [
    {
      path: 'products',
      component: ProductListComponent,
    },
    {
      path: 'product',
      component: ProductDetailComponent,
    },
    {
      path: 'createPolicy',
      component: CreatePolicyComponent,
    },
    {
      path: 'policyDetail',
      component: PolicyDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductRoutingModule {
}
