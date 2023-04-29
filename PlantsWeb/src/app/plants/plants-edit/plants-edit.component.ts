import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStructAdapter } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Plant } from 'src/app/models/Plant';
import { WebApiService } from 'src/app/webapi.service';
import { formatDate } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';

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
    name : ['', [Validators.required, Validators.maxLength(50)]],
    description : "",
    imageUrl: "" ,
    interval: 0,
    note: "", 
    lastNotification: formatDate( Date(), 'yyyy-MM-dd', 'en', '+0200')
  })

  constructor(
    private formBuilder: FormBuilder,
    private data: DataService,
    private router: Router,
    private route: ActivatedRoute,
    private webApi: WebApiService,
    private snackBar: MatSnackBar
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
             this.editForm.controls['lastNotification'].setValue('hm'),
             this.editForm.controls['note'].setValue(this.currentPlant.note)},
          error: (error) => { console.error("No plants with this id.", error)}
        });
      }

  }


  editPlant(){
   
    const modifiedPlant: Plant= new Plant(
      this.currentPlantId,
      this.editForm.value.name!,
      this.editForm.value.description!,
      this.editForm.value.imageUrl!,
      this.editForm.value.note!,
      this.editForm.value.interval!,
      new Date(this.editForm.value.lastNotification!),
      null,
      null,
      this.currentPlant.userId,
      );
      console.log(modifiedPlant)

      if (modifiedPlant == null){
        //snackbar -> unsuccesful add
      }else{
       
        this.webApi.editPlant(this.currentPlantId, modifiedPlant).subscribe({
          next: (result) => {  
           
            let index = this.plants.findIndex(p => p.id = result.id);
            this.plants.splice(index, 1, result);
            
            this.newMessage(this.plants), 
            this.router.navigate(['plants'])
            this.openSnackBar("Successfully edited plant!");},
            
          error: (error) => {
            this.openSnackBar("Couldn't edit plant. Try again!"),
            console.error('Editing failed', error)}
        });

      }
  }

  newMessage(updatedPlants : Plant[]) {
    this.data.changePlantsMessage(updatedPlants);
  }
  
  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
}
