import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http'
import { environment } from '../../../environments/environment';

const baseURL=environment.APP_BACKEND_URL ? environment.APP_BACKEND_URL : "/api";
const httpOptions={
  headers:new HttpHeaders({
    'content-type':'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  
  constructor(private http:HttpClient) { }

  getProducts(){
    return this.http.get(baseURL+"products");
  }

  getProduct(productCode:string){
    return this.http.get(baseURL+"products/" + productCode);
  }

  postOffer(offer:any):any{
    return this.http.post(baseURL+"offers",offer,httpOptions);
  }

  postPolicy(policy:any):any{
    return this.http.post(baseURL+"policies",policy,httpOptions);
  }

}