import { formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EventListenerOptions } from 'rxjs/internal/observable/fromEvent';
import { DataService } from 'src/app/data.service';
import { Plant } from 'src/app/models/Plant';
import { PlantDto } from 'src/app/models/PlantDto';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { UserService } from 'src/app/user.service';
import { WebApiService } from 'src/app/webapi.service';
@Component({
  selector: 'app-plants-add',
  templateUrl: './plants-add.component.html',
  styleUrls: ['./plants-add.component.css']
})
export class PlantsAddComponent implements OnInit{

  maintenance: boolean = false;
  currentUserId!: string;
  plants: Plant[] = [];
  subscription!: Subscription;
  
  fileName = '';
  file! : File;
  formData!: FormData;
  
  uploadSub! : Subscription | null ;

  addForm = this.formBuilder.group({
    name : ['', [Validators.required, Validators.maxLength(50)]],
    description : "",
    imageUrl: File,
    interval: [{value: 0, disabled: true}, [Validators.required, Validators.min(1)]],
    note: [{value: '', disabled: true},[Validators.required]],
    lastNotification: [{value: formatDate( Date(), 'yyyy-MM-dd', 'en', '+0000'), disabled: true }, [Validators.required]]
  })


  constructor(
    private formBuilder: FormBuilder,
    private data: DataService,
    private router: Router,
    private webApi: WebApiService,
    private snackBar: MatSnackBar,
    private route : ActivatedRoute,
    private userService : UserService
    ){
      this.currentUserId = this.userService.LoggedInUser();
    }



  ngOnInit(){
    console.log( this.maintenance)
    this.getPlantOfUser();

  }

  onFileChanged(event : any ){
    const file:File = event.target.files[0];

    if (file) {

        this.fileName = file.name;

        this.formData = new FormData();

        this.formData.append("image", file);
        console.log(this.formData.get("image"));
        this.file = file;
        
        const images = event.target.files;
        if (images.length === 0)
            return;
    }
  }
  cancelUpload() {
    this.fileName = ''; 
  }

  getPlantOfUser(){
    this.webApi.getPlantsOfUser(this.currentUserId).subscribe({
      next: (plants) => {this.plants = plants},
      error: (error) => {this.openSnackBar("Something went wrong. Try again!"), console.error('Getting plant for user failed.',error)}
    })
  }


  addPlant(){

   
    let date = new Date(this.addForm.value.lastNotification!);
    let utcDate = new Date(
      date.getUTCFullYear(), 
      date.getUTCMonth(),
      date.getUTCDate(),
      date.getUTCHours()+4,
      date.getUTCMinutes(),
      date.getUTCSeconds()
    )
      console.log("date entered: ", date);
      console.log("converted to utc: ", utcDate);

      let newPlant : PlantDto;
      if (this.maintenance){
        //yes maintenance
          newPlant = new PlantDto(
          "00000000-0000-0000-0000-000000000000",
          this.addForm.value.name!,
          this.addForm.value.description!,
          null,
          this.addForm.value.note!,
          this.addForm.value.interval!,
          utcDate,
          null ,
          
          this.currentUserId
          );
          console.log(newPlant);
      }else{
        //no maintenance
        newPlant = new PlantDto(
          "00000000-0000-0000-0000-000000000000",
          this.addForm.value.name!,
          this.addForm.value.description!,
          null,
          null,
          null,
          null,
          null,
          this.currentUserId
          );
      }
     

      if (newPlant != null){
        console.log("formdata: ", this.formData)
        this.webApi.addPlant(newPlant, this.formData).subscribe({
          next: (res) => {
            if (this.fileName.length > 0 || this.formData != undefined){
              this.webApi.addImage(this.formData, res.id).subscribe({
                next: (res) => {console.log("result: ", res)},
                error: (err) => {this.openSnackBar("couldnt upload picture")},
              }),
              this.plants.push(res), 
              this.newMessage(this.plants),
              this.router.navigate(['plants'])
              this.openSnackBar('Plant successfully created!');
            }
            else {
              this.plants.push(res), 
              this.newMessage(this.plants),
              this.router.navigate(['plants'])
              this.openSnackBar('Plant successfully created!')
            }

            this.plants.push(res), 
            this.newMessage(this.plants),
            this.router.navigate(['plants'])
            this.openSnackBar('Plant successfully created!');},
          error: (err) => { 
            this.openSnackBar("Couldn't add plant. Try again!"),
            console.error("Adding plant to database failed.", err)}
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
      this.addForm.controls['note'].enable();
      this.addForm.controls['lastNotification'].enable();
      this.addForm.controls['interval'].enable();
      
    }else{
      this.addForm.controls['note'].disable();
      this.addForm.controls['lastNotification'].disable();
      this.addForm.controls['interval'].disable();
    }
    
  }


  goBack(){
    this.router.navigate([`plants`])
  }
}