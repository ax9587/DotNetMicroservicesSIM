import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ngx-coverlist',
  templateUrl: './coverlist.component.html',
  styleUrls: ['./coverlist.component.scss']
})
export class CoverlistComponent implements OnInit {

  @Input() covers:any;
  constructor() { }

  ngOnInit(): void {
  }

}
