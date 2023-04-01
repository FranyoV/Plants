import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Plant } from '../../models/Plant';
import testData from '../../models/testData';
import { WebApiService } from 'src/app/web-api.service';

@Component({
  selector: 'app-plants',
  templateUrl: './plants-list.component.html',
  styleUrls: ['./plants-list.component.css']
})
export class PlantsListComponent {
  testData: Plant[] = testData;
  plants: Plant[] = [];

  constructor(
    private router: Router
   
        ){}

  ngOnit(){
   /* this.webApi.getPlants().subscribe({
      next: (plants) => {this.plants = plants},
      error: (error) => {console.error('Getting plant for user failer',error)}
    })*/
  }

  goToEditPlantPage(){
    this.router.navigate(['plantedit']);
  }

  goToAddPlantPage(){
    this.router.navigate(['plantadd']);
  }
}
