import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Plant } from './models/Plant';
import testData from './models/testData';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  plants : Plant[] = [];

  private dataSource = new BehaviorSubject<Plant[]>(this.plants);
  private plantId = new BehaviorSubject<Guid>(Guid.create());

  currentData = this.dataSource.asObservable();
  currentPlantId = this.plantId.asObservable();

  constructor() { }

  updateData(data: Plant[]){
    this.dataSource.next(data);
    console.log("message in dataservice");
  }

  sendPlantId(id: Guid){
    this.plantId.next(id);
    console.log("plant id: ", id)
  }
}
