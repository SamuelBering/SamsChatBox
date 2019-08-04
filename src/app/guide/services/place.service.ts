import { Injectable } from '@angular/core';
import { Http, URLSearchParams, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { BaseService } from '../../shared/services/base.service';

// import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';
import { Place } from '../models/place.interface';
import { ConfigService } from '../../shared/utils/config.service';

@Injectable()
export class PlaceService extends BaseService {

    // baseUrl: string = 'https://maps.googleapis.com/maps/api/place/nearbysearch/json';
    // apiKey: string = 'AIzaSyCxKlIFM4NyNdg6o6SJV6_lW6ryG0m019A';
    baseUrl: string = '';

    constructor(private http: Http, private configService: ConfigService) {
        super();
        this.baseUrl = configService.getApiURI();
    }

    GetPlaces(): Observable<Array<Place>> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let search = new URLSearchParams();
        search.set('Lat', '123');
        search.set('Long', '123');
        search.set('Radius', '123');
        return this.http.get(`${this.baseUrl}/place/getplaces`, { search: search, headers: headers })
            .map(res => {
                return res.json(); 
            })
            .catch(this.handleError);
    }

    // GetPlaces(): Observable<string> {

    //     const places = new Observable<string>((observer) => {
    //         observer.next('Staty');
    //         observer.next('Banan');
    //         observer.next('Kalle');
    //         observer.complete();
    //         // When the consumer unsubscribes, clean up data ready for next subscription.
    //         return { unsubscribe() { } };
    //     });

    //     return places;


    // }



}
