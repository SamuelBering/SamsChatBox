import { Component, OnInit, Input } from '@angular/core';
import { Place } from '../models/place.interface';
import { Filter } from '../models/filter.interface';
import { PlaceService } from '../services/place.service';

@Component({
  selector: 'places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.scss']
})
export class PlacesComponent implements OnInit {

  @Input() filter: Filter;

  places: Array<Place> = [{ name: "Kyrka" }, { name: 'Hus' }];

  constructor(private placeService: PlaceService) { }

  ngOnInit() {
    // this.placeService.GetPlaces().subscribe((place) => {
    //   this.places[0].name = place;
    // });

    this.placeService.GetPlaces2().subscribe((place) => {
      this.places[0].name = place;
    });

  }

}
