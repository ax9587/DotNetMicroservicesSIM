import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../@core/services/product.service';

@Component({
  selector: 'ngx-create-policy',
  templateUrl: './create-policy.component.html',
  styleUrls: ['./create-policy.component.scss']
})
export class CreatePolicyComponent implements OnInit {
  public policyForm=this.formBuilder.group({
    firstName:['',[Validators.required]],
    lastName:['',[Validators.required]],
    taxId:['',[Validators.required]],
    country:['',[Validators.required]],
    zipCode:['',[Validators.required]],
    city:['',[Validators.required]],
    street:['',[Validators.required]],
  });
  offerNumber:string;
  policyNumber:string;
  policyHolder= {
    firstName: '',
    lastName: '',
    taxId: ''
  };
  policyAddress={
    country: 'Poland',
    zipCode: '',
    city: '',
    street: ''
  };
  countries= [
    'Poland',
    'France',
    'Germany'
  ];

  constructor(private route: ActivatedRoute,private router: Router,private formBuilder:FormBuilder,private httpService: ProductService,) { 
    this.offerNumber = this.route.snapshot.paramMap.get('offerNumber');
    console.log(this.offerNumber);
  }

  ngOnInit(): void {
  }

  postPolicy(){
    console.log('postpolicy');

    this.policyHolder.firstName=this.policyForm.controls["firstName"].value;
    this.policyHolder.lastName=this.policyForm.controls["lastName"].value;
    this.policyHolder.taxId=this.policyForm.controls["taxId"].value;
    
    this.policyAddress.country=this.policyForm.controls["country"].value;
    this.policyAddress.zipCode=this.policyForm.controls["zipCode"].value;
    this.policyAddress.city=this.policyForm.controls["city"].value;
    this.policyAddress.street=this.policyForm.controls["street"].value;

    const request = {
      offerNumber: this.offerNumber,
      policyHolder: this.policyHolder,
      policyHolderAddress: this.policyAddress
    };
    console.log(request);

    this.httpService.postPolicy(request).subscribe(
      (response) => { 
        console.log(response);
        this.policyNumber = response.policyNumber;
        this.router.navigate(['/pages/product/policyDetail', { policyNumber: this.policyNumber }  ]);
      },
      (error) => { console.log(error); });
    
   }
}
