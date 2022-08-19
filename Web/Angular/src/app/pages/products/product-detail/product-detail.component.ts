import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,FormArray  } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../@core/services/product.service';

@Component({
  selector: 'ngx-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {

  public productForm=this.formBuilder.group({
    policyFrom:['',[Validators.required]],
    policyTo:['',[Validators.required]],
    answercontrols: this.formBuilder.array([
    ])
  });
  get answercontrols() {
    return this.productForm.get('answercontrols') as FormArray;
  }
  productCode:string;
  productDetails:any;
  answers: any;


  
  mode: string='EDIT';
  price= {
          amountToPay: null
         };
  policyFrom= '';
  policyTo= '';
  offerNumber= '';

  constructor(private formBuilder:FormBuilder,private activatedRoute: ActivatedRoute,private httpService: ProductService,private router: Router ) {
    this.activatedRoute.queryParams.subscribe(params => {
      console.log(params['productCode']);
      this.productCode=params['productCode'];
      });
   }

  ngOnInit(): void {

    this.httpService.getProduct(this.productCode).subscribe(
      (response) => { 
                this.productDetails = response; 
                if (!this.productDetails.questions)
                    return;
                console.log(this.productDetails.questions.length);
                this.answers=[];
                for (let i = 0; i < this.productDetails.questions.length; i++) {
                    this.answers.push({
                        answer: null,
                        question: this.productDetails.questions[i]
                    });
                    this.answercontrols.push(this.formBuilder.control(''));
                };
      },
      (error) => { console.log(error); });


  }

  onSubmit(){

    console.log("On-Submit");
    this.httpService.postOffer(this.createRequest()).subscribe(
      (response) => { 
        console.log(response);
        this.productForm.controls['policyFrom'].disable();
        this.productForm.controls['policyTo'].disable();
        for (const field in this.answercontrols.controls) { 
          const control = this.answercontrols.get(field);  
          control.disable();
        }
        this.mode = 'VIEW';
        this.price.amountToPay = response.totalPrice;
        this.offerNumber = response.offerNumber;
      },
      (error) => { console.log(error); });
 }

  backToEdit() {
  this.mode = 'EDIT';
  this.productForm.controls['policyFrom'].enable();
  this.productForm.controls['policyTo'].enable();
  for (const field in this.answercontrols.controls) { 
    const control = this.answercontrols.get(field);  
    control.enable();
  }
 }

  createRequest() {
    const request = {
        'productCode': this.productDetails.code,
        'policyFrom': this.productForm.controls["policyFrom"].value,
        'policyTo': this.productForm.controls["policyTo"].value,
        'selectedCovers': [],
        'answers': []
    };

    for (let i = 0; i < this.productDetails.covers.length; i++) {
        request.selectedCovers.push(this.productDetails.covers[i].code);
    }

    console.log(this.answercontrols);
    for (let j = 0; j < this.answers.length; j++) {
        request.answers.push({
            'questionCode': this.answers[j].question.questionCode,
            'questionType': this.answers[j].question.questionType,
            //'answer': this.answers[j].question.questionType==='Numeric' ? parseInt(this.answercontrols[j].value) : this.answercontrols[j].value
            'answer':  this.answercontrols.controls[j].value
        });
    }
    console.log(request);
    return request;
   }

   createPolicy(){
    console.log('route to policy');
    this.router.navigate(['/pages/product/createPolicy', { offerNumber: this.offerNumber }  ]);
   }

}
