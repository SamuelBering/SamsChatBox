import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataCollectorService } from '../services/data.collector.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {

  errors: string;
  
  constructor(route: ActivatedRoute, dataCollectorService: DataCollectorService) {
    let errorKey: string = route.snapshot.params['errorKey'];
    this.errors = dataCollectorService.storage[errorKey];
  }

  ngOnInit() {
  }

}
