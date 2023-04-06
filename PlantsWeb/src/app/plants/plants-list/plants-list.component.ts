import { Component, OnDestroy, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { Plant } from '../../models/Plant';
import testData from '../../models/testData';
import { WebApiService } from 'src/app/webapi.service';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Guid } from 'guid-typescript';
import { MatDialog } from '@angular/material/dialog';
import {MatMenuTrigger} from '@angular/material/menu';
import { Dialog } from '@angular/cdk/dialog';
import { DialogComponent } from 'src/app/dialog/dialog.component';

@Component({
  selector: 'app-plants',
  templateUrl: './plants-list.component.html',
  styleUrls: ['./plants-list.component.css']
})
export class PlantsListComponent implements OnInit, OnDestroy {
  //testData: Plant[] = testData;

  plants: Plant[] = [];
  subscription!: Subscription;
  menuTrigger!: MatMenuTrigger;


  constructor(
    private router: Router,
    private data: DataService,
    private webApi: WebApiService,
    private dialog: MatDialog
    ){}


  ngOnInit(){
    
    this.webApi.getPlantsOfUser(Guid.parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")).subscribe({
      next: (plants) => {this.plants = plants},
      error: (error) => {console.error('Getting plant for user failed.',error)}
    })

    this.subscription = this.data.currentData.subscribe(
      (plants ) => {this.plants = plants; 
        console.log("in the listing component",this.plants);
        
      }
    )    
    
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  deletePlant(plantId: Guid){

    console.log("delete clicked.")
    this.webApi.deletePlant(plantId).subscribe({
      next: (result) => {
        const index = this.plants.findIndex(p => p.id == plantId);
        this.plants.splice(index, 1)}
    })
  }

  goToEditPlantPage(plantId: Guid){
    console.log(plantId);
    this.data.sendPlantId(plantId);
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

}
