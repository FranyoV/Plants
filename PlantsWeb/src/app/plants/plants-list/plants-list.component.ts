import { Component, OnDestroy, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { Plant } from '../../models/Plant';
import { WebApiService } from 'src/app/webapi.service';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Guid } from 'guid-typescript';
import { MatDialog } from '@angular/material/dialog';
import {MatMenuTrigger} from '@angular/material/menu';
import { Dialog } from '@angular/cdk/dialog';
import { DialogComponent } from 'src/app/plants/plants-list/dialog/dialog.component';

@Component({
  selector: 'app-plants',
  templateUrl: './plants-list.component.html',
  styleUrls: ['./plants-list.component.css']
})
export class PlantsListComponent implements OnInit, OnDestroy {

  plants: Plant[] = [];
  subscription!: Subscription;
  menuTrigger!: MatMenuTrigger;


  constructor(
    private router : Router,
    private data : DataService,
    private webApi : WebApiService,
    private dialog : MatDialog
    ){}


  ngOnInit(){

    this.subscription = this.data.currentPlantsMessage.subscribe( message => this.plants = message ) ;

    this.webApi.getPlantsOfUser("3fa85f64-5717-4562-b3fc-2c963f66afa6").subscribe({
      next: (result) => {this.plants = result},
      error: (error) => {console.error('Getting plant for user failed.',error)}
    }) 
    
  }

  deletePlant(plantId: string){

    this.openDialog("1000", '1000') ;

    this.webApi.deletePlant(plantId).subscribe({
      next: (result) => {
        const index = this.plants.findIndex(p => p.id == plantId);
        this.plants.splice(index, 1)}
    })
  }

  goToEditPlantPage(plantId: string){
    this.router.navigate([`plants/${plantId}`]);

  }

  goToAddPlantPage(){
    this.router.navigate(['plant/new']);
  }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {

  //  const dialogRef = this.dialog.open(DialogComponent)
    this.dialog.open( DialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
      
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
