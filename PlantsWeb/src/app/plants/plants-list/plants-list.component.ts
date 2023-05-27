import { Component, OnDestroy, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Plant } from '../../models/Plant';
import { WebApiService } from 'src/app/webapi.service';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Guid } from 'guid-typescript';
import { MatDialog } from '@angular/material/dialog';
import {MatMenuTrigger} from '@angular/material/menu';
import { Dialog } from '@angular/cdk/dialog';
import { DialogComponent } from 'src/app/plants/plants-list/dialog/dialog.component';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from 'src/app/user.service';

@Component({
  selector: 'app-plants',
  templateUrl: './plants-list.component.html',
  styleUrls: ['./plants-list.component.css']
})
export class PlantsListComponent implements OnInit, OnDestroy {

  currentUserId!: string;
  plants: Plant[] = [];
  subscription!: Subscription;
  menuTrigger!: MatMenuTrigger;


  constructor(
    private router : Router,
    private data : DataService,
    private webApi : WebApiService,
    private dialog : MatDialog,
    private snackBar : MatSnackBar,
    private route : ActivatedRoute,
    private userService : UserService
    ){
      this.currentUserId = this.userService.LoggedInUser();
    }


  ngOnInit(){

    this.subscription = this.data.currentPlantsMessage.subscribe( message => this.plants = message ) ;
    this.getPlantsOfUser();

  }

  getPlantsOfUser(){
    this.webApi.getPlantsOfUser(this.currentUserId).subscribe({
      next: (result) => {this.plants = result},
      error: (error) => {this.openSnackBar("Something went wrong. Try again!"), console.error('Getting plant for user failed.',error)}
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


  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    this.dialog.open( DialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
      
    });
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

  goToEditPlantPage(plantId: string){
    this.router.navigate([`plants/${plantId}`]);

  }

  goToAddPlantPage(){
    this.router.navigate([`plant/new`]);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
