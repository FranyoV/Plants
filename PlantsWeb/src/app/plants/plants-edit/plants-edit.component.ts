import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Plant } from 'src/app/models/Plant';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-plants-edit',
  templateUrl: './plants-edit.component.html',
  styleUrls: ['./plants-edit.component.css']
})
export class PlantsEditComponent implements OnInit{

  subscription! : Subscription;
  currentPlantId : string = ""; 
  currentPlant!: Plant;
  plants: Plant[] = [];

  editForm = this.formBuilder.group({
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
    private route: ActivatedRoute,
    private webApi: WebApiService
  ){}


  ngOnInit(){
    this.data.currentPlantsMessage.subscribe( message => this.plants = message );

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("plantId");
      this.currentPlantId = id!;})


      if(this.currentPlant == null){
        this.webApi.getPlantById(this.currentPlantId).subscribe({
          next: (result) => {
             this.currentPlant = result; console.log("currentplant:", this.currentPlant);
             this.editForm.controls['name'].setValue(this.currentPlant.name);
             this.editForm.controls['description'].setValue(this.currentPlant.description);
             this.editForm.controls['imageUrl'].setValue(this.currentPlant.imageUrl ),
             this.editForm.controls['interval'].setValue(this.currentPlant.interval ),
             this.editForm.controls['note'].setValue(this.currentPlant.note)},
          error: (error) => { console.error("No plants with this id.", error)}
        });
      }

  }




  editPlant(){
    console.log("currentplantid in edit:", this.currentPlantId);
    const modifiedPlant: Plant= new Plant(
      this.currentPlantId,
      this.editForm.value.name!,
      this.editForm.value.description!,
      this.editForm.value.imageUrl!,
      this.editForm.value.note!,
      this.editForm.value.interval!,
      null,
      null,
      null,
      this.currentPlant.userId,
      );

      if (modifiedPlant == null){
        //snackbar -> unsuccesful add
      }else{
       
        this.webApi.editPlant(this.currentPlantId, modifiedPlant).subscribe({
          next: (result) => {  
           
            let index = this.plants.findIndex(p => p.id = result.id);
            this.plants.splice(index, 1, result);
            
            this.newMessage(this.plants), 
            this.router.navigate(['plants']);},
            
          error: (error) => {console.error('Adding failed', error)}
        });

      }
  }

  newMessage(updatedPlants : Plant[]) {
    this.data.changePlantsMessage(updatedPlants);
  }
  
}
