import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductCardComponent } from './product-card/product-card.component';
import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product.component';
import { NbButtonModule, NbCardModule, } from '@nebular/theme';
import { CoverlistComponent } from './coverlist/coverlist.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import {
  NbInputModule,
} from '@nebular/theme';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreatePolicyComponent } from './create-policy/create-policy.component';
import { PolicyDetailComponent } from './policy-detail/policy-detail.component';


@NgModule({
  declarations: [
    ProductListComponent,
    ProductCardComponent,
    ProductComponent,
    CoverlistComponent,
    ProductDetailComponent,
    CreatePolicyComponent,
    PolicyDetailComponent,
  ],
  imports: [
    FormsModule, 
    ReactiveFormsModule,
    NbInputModule,
    NbCardModule,
    NbButtonModule,
    CommonModule,
    ProductRoutingModule,
  ]
})
export class ProductModule { }
