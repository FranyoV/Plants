import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Maintenance } from 'src/app/models/Maintenance';
import { Plant } from 'src/app/models/Plant';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-plants-edit',
  templateUrl: './plants-edit.component.html',
  styleUrls: ['./plants-edit.component.css']
})
export class PlantsEditComponent implements OnInit, OnDestroy{

  subscription! : Subscription;
  currentPlantId : Guid = Guid.createEmpty(); 
  currentPlant!: Plant;
  plants: Plant[] = [];

  editForm = this.formBuilder.group({
    id : "",
    name : "",
    description : "",
    imageUrl: "" ,
    userId: ""
  })

  constructor(
    private formBuilder: FormBuilder,
    private data: DataService,
    private router: Router,
    private webApi: WebApiService
  ){}


  ngOnInit(){
    this.subscription = this.data.currentPlantId.subscribe(
      plantId => this.currentPlantId = plantId);

    this.webApi.getPlantById(this.currentPlantId).subscribe({
      next: (result) => {
         this.currentPlant = result; console.log("currentplant:", this.currentPlant);
         this.editForm.controls['name'].setValue(this.currentPlant.name);
         this.editForm.controls['description'].setValue(this.currentPlant.description);
         this.editForm.controls['imageUrl'].setValue(this.currentPlant.imageUrl )},
      error: (error) => { console.error("No plants with this id.", error)}
    });

    console.log("currentplant outside of get:", this.currentPlant);
   
  }


  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  editPlant(){
    console.log("currentplantid in edit:", this.currentPlantId);
    const modifiedPlant: Plant= new Plant(
      this.currentPlantId,
      this.editForm.value.name!,
      this.editForm.value.description!,
      this.editForm.value.imageUrl!,
      this.currentPlant.userId
      );

      if (modifiedPlant == null){
        //snackbar -> unsuccesful add
      }else{
        console.log("modifiedplant:", modifiedPlant);
        this.webApi.editPlant(this.currentPlantId, modifiedPlant).subscribe({
          next: () => {
            
      let index = this.plants.findIndex(p => p.id = modifiedPlant.id);
      this.plants.splice(index, 1, modifiedPlant);
      this.data.updateData(this.plants);

},
          error: (error) => {console.log('Adding failed', error)}
        });
      }

      this.router.navigate(['plants']);
    
  }
}
