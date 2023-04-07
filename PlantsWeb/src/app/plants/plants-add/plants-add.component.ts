import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Maintenance } from 'src/app/models/Maintenance';
import { Plant } from 'src/app/models/Plant';
import { WebApiService } from 'src/app/webapi.service';
import { MatFormField} from '@angular/material/form-field';
import { PlantDto } from 'src/app/models/PlantDto';

@Component({
  selector: 'app-plants-add',
  templateUrl: './plants-add.component.html',
  styleUrls: ['./plants-add.component.css']
})
export class PlantsAddComponent implements OnInit,OnDestroy{
  //@Input() plants: Plant[] = [];
  //@Output() updateList = new EventEmitter<Plant[]>();
  //plants: Plant[] = [];

  plants: Plant[] = [];
  subscription! : Subscription;

  addForm = this.formBuilder.group({
    id : "",
    name : "",
    description : "",
    imageUrl: "" ,
    interval: 0,
    note: "",
    lastNotification: Date
  })


  constructor(
    private formBuilder: FormBuilder,
    private data: DataService,
    private router: Router,
    private webApi: WebApiService
  ){}


  ngOnInit(){
    this.subscription = this.data.currentPlants.subscribe(
      plants => this.plants = plants)
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  addPlant(){
    const newPlant: Plant = new Plant(
      "00000000-0000-0000-0000-000000000000",
      this.addForm.value.name!,
      this.addForm.value.description!,
      this.addForm.value.imageUrl!,
      this.addForm.value.note!,
      this.addForm.value.interval!,
      null,
      null,
      null,
      "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      );

      console.log(newPlant);

      if (newPlant == null){
        //snackbar -> unsuccesful add
      }else{
        this.webApi.addPlant(newPlant).subscribe({
          next: (result) => {
            this.plants.push(newPlant), 
            this.data.updatePlantsList(this.plants);
            console.log("new plant added:" , result)},
          error: (error) => { console.error("Adding plant to database failed.", error)}
        });

        this.plants.push(newPlant);
        this.data.updatePlantsList(this.plants);
       
      }
      this.router.navigate(['plants']);
  }

}