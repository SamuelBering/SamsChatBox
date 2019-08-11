import { Injectable } from '@angular/core';
import { Http, URLSearchParams, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { BaseService } from '../../shared/services/base.service';

// import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';
import { Place } from '../models/place.interface';
import { ConfigService } from '../../shared/utils/config.service';
import { Filter } from '../models/filter.interface';

@Injectable()
export class PlaceService extends BaseService {

    // baseUrl: string = 'https://maps.googleapis.com/maps/api/place/nearbysearch/json';
    // apiKey: string = 'AIzaSyCxKlIFM4NyNdg6o6SJV6_lW6ryG0m019A';
    baseUrl: string = '';

    constructor(private http: Http, private configService: ConfigService) {
        super();
        this.baseUrl = configService.getApiURI();
    }

    private CreateQueryParameters(filter: Filter): URLSearchParams {
        let queryParams = new URLSearchParams();

        for (let propertyName in filter) {
            queryParams.set(propertyName, filter[propertyName]);
        }

        return queryParams;
    }

    // GetPlaces2(filter: Filter): Observable<Array<Place>> {
    //     let pyrmont = new google.maps.LatLng(-33.8665433,151.1956316);

    //     let map = new google.maps.Map(document.getElementById('map'), {
    //         center: pyrmont,
    //         zoom: 15
    //       });
      
    //     var request = {
    //       location: pyrmont,
    //       radius: '500',
    //       query: 'restaurant'
    //     };
      
    //     var service = new google.maps.places.PlacesService(map);
    //     service.nearbySearch(request, callback);
    // }

    // callback(results, status) {
    //     if (status == google.maps.places.PlacesServiceStatus.OK) {
    //       for (var i = 0; i < results.length; i++) {
    //         var place = results[i];
    //         console.log(results[i]);
    //       }
    //     }
    //   }

    GetPlaces(filter: Filter): Observable<Array<Place>> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let queryParams = this.CreateQueryParameters(filter);

        return this.http.get(`${this.baseUrl}/place/getplaces`, { search: queryParams, headers: headers })
            .map(res => {
                return res.json();
            })
            .catch(this.handleError);
    }  

}
