import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Filter } from '../models/filter.interface';
// import { EventEmitter } from 'protractor';

@Component({
  selector: 'filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {
  
  @Output() filter: EventEmitter<Filter> = new EventEmitter();
  keywordInput: FormControl = new FormControl('');

  private currentFilter: Filter = {
    lat: 0,
    long: 0,
    radius: 0,
    keyword: '',
    language: '',
    type: ''
  };

  constructor() {
    this.keywordInput.valueChanges
      .debounceTime(500)
      .subscribe(this.onSearchInputValueChanges.bind(this));
  }

  onSearchInputValueChanges(keyword) {
    this.currentFilter.keyword = keyword;
    let filter: Filter = Object.create(this.currentFilter);
    this.filter.emit(filter);
  }

  ngOnInit() {
  }

}
