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
import { UserService } from 'src/app/user.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { PlantDto } from 'src/app/models/PlantDto';
import { resolve } from 'chart.js/dist/helpers/helpers.options';

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

  fileName = '';
  file! : File;
  formData!: FormData;

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
    private snackBar: MatSnackBar,
    private userService : UserService,
    private sanitizer: DomSanitizer
    ){
      this.currentUserId = this.userService.LoggedInUser();
    }


  ngOnInit(){
    
    this.data.currentPlantsMessage.subscribe( message => this.plants = message );

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("plantId");
      this.currentPlantId = id!;})


      if(this.currentPlant == null){
        this.webApi.getPlantById(this.currentPlantId).subscribe({
          next: (result) => {
             this.currentPlant = result; 
             
             this.editForm.controls['name'].setValue(this.currentPlant.name);
             this.editForm.controls['description'].setValue(this.currentPlant.description);
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
      //maintenance
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

      if (modifiedPlant != null){     
        this.webApi.editPlant(this.currentPlantId, modifiedPlant).subscribe({
          next: (result) => {  
            if (this.fileName.length > 0  || this.formData != undefined){
              this.addImage(result);
            }else{
              let index = this.plants.findIndex(p => p.id = result.id);
              this.plants.splice(index, 1, result);
              this.newMessage(this.plants), 
              this.router.navigate(['plants'])
              this.openSnackBar("Successfully edited plant!")
            }
          },
          error: (error) => {
            this.openSnackBar("Couldn't edit plant. Try again!"),
            console.error('Editing failed', error)}
        });

      }
  }


    addImage(plant : Plant){
      this.webApi.addImage(this.formData, plant.id).subscribe({
        next: (res) => {
          let index = this.plants.findIndex(p => p.id = plant.id);
          this.plants.splice(index, 1, plant);
          
          this.newMessage(this.plants), 
          this.router.navigate(['plants'])
          this.openSnackBar("Successfully edited plant!")
        },
        error: (err) => {this.openSnackBar("Image upload was unsuccessful. Try again.")}
      })
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
    this.maintenance = !this.maintenance;
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

  
  onFileChanged(event : any ){
    const file:File = event.target.files[0];

    if (file) {
        this.fileName = file.name;
        this.formData = new FormData();
        this.formData.append("image", file);
        this.file = file;
        
        const images = event.target.files;
        if (images.length === 0)
            return;
    }
  }

  cancelUpload() {  
    this.fileName = '';
  }


  goBack(){
    this.router.navigate([`plants`])
  }
}
