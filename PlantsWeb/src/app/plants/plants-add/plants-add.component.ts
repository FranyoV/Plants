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
  
  imagePath:string = '';
  url!:ArrayBuffer|null|string;
  fileName = '';
  file! : File;
  
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

        const formData = new FormData();

        formData.append("thumbnail", file);
        console.log(formData.get("thumbnail"));
        this.file = file
        //const upload$ = this.http.post("/api/thumbnail-upload", formData);
        const files = event.target.files;
        if (files.length === 0)
            return;
    
        const mimeType = files[0].type;
        if (mimeType.match(/image\/*/) == null) {
            this.openSnackBar("Only images are supported.");
            return;
        }
    
        const reader = new FileReader();
        this.imagePath = files;
        reader.readAsDataURL(files[0]); 
        reader.onload = (_event) => { 
            this.url = reader.result; 
            //this.addForm.controls['imageUrl'].setValue(this.url)
        }
        console.log(this.url)
        
    }
  }
  cancelUpload() {
    this.fileName = '';
    this.url = '';
    
  }

  getPlantOfUser(){
    this.webApi.getPlantsOfUser(this.currentUserId).subscribe({
      next: (plants) => {this.plants = plants},
      error: (error) => {this.openSnackBar("Something went wrong. Try again!"), console.error('Getting plant for user failed.',error)}
    })
  }


  addPlant(){
    console.log(this.url)
    console.log("file: ", this.file)
   // this.addForm.controls['imageUrl'].setValue(this.file);
   
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

      let newPlant : Plant;
      if (this.maintenance){
        //yes maintenance
          newPlant = new Plant(
          "00000000-0000-0000-0000-000000000000",
          this.addForm.value.name!,
          this.addForm.value.description!,
          this.file,
          this.addForm.value.note!,
          this.addForm.value.interval!,
          utcDate,
          null ,
          null,
          this.currentUserId
          );
          console.log(newPlant);
      }else{
        //no maintenance
        newPlant = new Plant(
          "00000000-0000-0000-0000-000000000000",
          this.addForm.value.name!,
          this.addForm.value.description!,
          this.file!,
          null,
          null,
          null,
          null ,
          null,
          this.currentUserId
          );
      }
     

      console.log(newPlant);
      if (newPlant == null){
        //snackbar -> unsuccesful add
      }else{
        this.webApi.addPlant(newPlant).subscribe({
          next: (res) => {
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