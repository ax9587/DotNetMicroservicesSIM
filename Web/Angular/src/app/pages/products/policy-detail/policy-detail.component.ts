import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-policy-detail',
  templateUrl: './policy-detail.component.html',
  styleUrls: ['./policy-detail.component.scss']
})
export class PolicyDetailComponent implements OnInit {
  policyNumber:string;
  constructor(private route: ActivatedRoute) { 
    this.policyNumber = this.route.snapshot.paramMap.get('policyNumber');
    console.log(this.policyNumber);
  }

  ngOnInit(): void {
  }

}
