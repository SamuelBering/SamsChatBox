import { Component, OnInit } from '@angular/core';
import { Filter } from '../models/filter.interface';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  filter: Filter = {
    lat: 0,
    long: 0,
    radius: 0,
    keyword: '',
    language: '',
    type: ''
  };

  constructor() { }

  ngOnInit() {
  }

  onFilterChange(filter: Filter) {
    this.filter = filter;
  }
}
