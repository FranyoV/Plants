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

  maintenance : boolean = false;
  subscription! : Subscription;
  currentPlantId : string = ""; 
  currentPlant!: Plant;
  currentUserId : string = '';
  plants: Plant[] = [];


  editForm = this.formBuilder.group({
    name : ['', [Validators.required, Validators.maxLength(50)]],
    description : "",
    imageUrl: '',
    interval: [{value: 0, disabled: true}, [Validators.required, Validators.min(1)]],
    note: [{value: '', disabled: true},[Validators.required]],
    lastNotification: [{value: formatDate( Date(), 'yyyy-MM-dd', 'en', '+0000'), disabled: true }, [Validators.required]]
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

    this.route.parent?.params.subscribe({
      next: (params) => {
        const id = params["userId"];
        this.currentUserId = id}
      
    });

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("plantId");
      this.currentPlantId = id!;})


      if(this.currentPlant == null){
        this.webApi.getPlantById(this.currentPlantId).subscribe({
          next: (result) => {
             this.currentPlant = result; 
             console.log("currentplant:", this.currentPlant);
             this.editForm.controls['name'].setValue(this.currentPlant.name);
             this.editForm.controls['description'].setValue(this.currentPlant.description);
             //this.editForm.controls['imageUrl'].setValue(this.currentPlant.imageUrl ),
             this.editForm.controls['interval'].setValue(this.currentPlant.interval ),
             this.editForm.controls['lastNotification'].setValue(this.currentPlant.lastNotification?.toLocaleString()!),
             this.editForm.controls['note'].setValue(this.currentPlant.note)},
          error: (error) => { console.error("No plants with this id.", error)}
        });
      }

  }


  editPlant(){
    let date = new Date(this.editForm.value.lastNotification!);
    let utcDate = new Date(
      date.getUTCFullYear(), 
      date.getUTCMonth(),
      date.getUTCDate(),
      date.getUTCHours()+4,
      date.getUTCMinutes(),
      date.getUTCSeconds()
    )

    let modifiedPlant : Plant;
    if (this.maintenance){
      //yes maintenance
      modifiedPlant = new Plant(
        this.currentPlantId,
        this.editForm.value.name!,
        this.editForm.value.description!,
        null,
        this.editForm.value.note!,
        this.editForm.value.interval!,
        utcDate,
        null ,
        null,
        this.currentPlant.userId
        );
    }else{
      //no maintenance
      modifiedPlant = new Plant(
        this.currentPlantId,
        this.editForm.value.name!,
        this.editForm.value.description!,
        this.editForm.value.imageUrl!,
        null,
        null,
        null,
        null ,
        null,
        this.currentPlant.userId
        );
    }
   
   
   /* const modifiedPlant: Plant= new Plant(
      this.currentPlantId,
      this.editForm.value.name!,
      this.editForm.value.description!,
      this.editForm.value.imageUrl!,
      this.editForm.value.note!,
      this.editForm.value.interval!,
      utcDate,
      null,
      null,
      this.currentPlant.userId,
      );*/
      console.log(modifiedPlant)
      console.log(new Date(this.editForm.value.lastNotification!).getDate());

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

  showMaintenance(){
    console.log("before: ", this.maintenance)
    this.maintenance = !this.maintenance;
    console.log("after: ", this.maintenance)
    if(this.maintenance){
      this.editForm.controls['note'].enable();
      this.editForm.controls['lastNotification'].enable();
      this.editForm.controls['interval'].enable();
      
    }else{
      this.editForm.controls['note'].disable();
      this.editForm.controls['lastNotification'].disable();
      this.editForm.controls['interval'].disable();
    }
    
  }

  goBack(){
    this.router.navigate([`${this.currentUserId}/plants`])
  }
}
