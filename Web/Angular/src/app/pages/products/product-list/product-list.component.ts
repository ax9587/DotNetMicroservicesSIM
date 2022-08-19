import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../@core/services/product.service';

@Component({
  selector: 'ngx-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {

  products:any;
  constructor(private httpService: ProductService) { }

  ngOnInit(): void {
    this.httpService.getProducts().subscribe(
      (response) => { this.products = response; },
      (error) => { console.log(error); });
  }

}
