import { formatDate } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Plant } from 'src/app/models/Plant';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-plants-add',
  templateUrl: './plants-add.component.html',
  styleUrls: ['./plants-add.component.css']
})
export class PlantsAddComponent implements OnInit{

  plants: Plant[] = [];
  subscription!: Subscription;

  addForm = this.formBuilder.group({
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
    private webApi: WebApiService,
    private snackBar: MatSnackBar
  ){}


  ngOnInit(){

    this.webApi.getPlantsOfUser("3fa85f64-5717-4562-b3fc-2c963f66afa6").subscribe({
      next: (plants) => {this.plants = plants},
      error: (error) => {console.error('Getting plant for user failed.',error)}
    })

  }


  addPlant(){
    const newPlant: Plant = new Plant(
      "00000000-0000-0000-0000-000000000000",
      this.addForm.value.name!,
      this.addForm.value.description!,
      this.addForm.value.imageUrl!,
      this.addForm.value.note!,
      this.addForm.value.interval!,
      new Date(this.addForm.value.lastNotification!),
      null ,
      null,
      "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      );


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

}