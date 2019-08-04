import { Injectable } from '@angular/core';

@Injectable()
export class DataCollectorService {

    storage : { [key:string]: any } = [];

    constructor() { 
    }
}
