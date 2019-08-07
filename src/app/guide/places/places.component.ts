import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Place } from '../models/place.interface';
import { Filter } from '../models/filter.interface';
import { PlaceService } from '../services/place.service';
import { DataCollectorService } from '../services/data.collector.service';
import { Router } from '@angular/router';

@Component({
  selector: 'places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.scss']
})
export class PlacesComponent implements OnInit, OnChanges {
 
  @Input() filter: Filter;

  places: Array<Place> = [];

  constructor(private placeService: PlaceService, private dataCollectorService: DataCollectorService, private router: Router) { }

  ngOnInit() {
    this.getPlaces();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getPlaces();
  }

  private getPlaces(){
    this.placeService.GetPlaces(this.filter).subscribe((places) => {
      this.places = places;
    },
      errors => {
        let errorKey: string = 'placeServiceError';
        this.dataCollectorService.storage[errorKey] = errors;
        this.router.navigate(['/guide/error', errorKey]);
      });
  }
  
}
