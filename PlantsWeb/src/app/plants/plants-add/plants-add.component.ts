import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Maintenance } from 'src/app/models/Maintenance';
import { Plant } from 'src/app/models/Plant';
import { WebApiService } from 'src/app/web-api.service';

@Component({
  selector: 'app-plants-add',
  templateUrl: './plants-add.component.html',
  styleUrls: ['./plants-add.component.css']
})
export class PlantsAddComponent {
  @Input() plants: Plant[] = [];
  @Output() updateList = new EventEmitter<Plant[]>();
  //plants: Plant[] = [];

  addForm = this.formBuilder.group({
    id : "",
    name : "",
    description : "",
    maintenance: new Maintenance(),
    imageUrl: "" ,
    userId: "0"
  })


  constructor(
    private formBuilder: FormBuilder
  ){}

  addPlant(){
    //var newPlant: Plant;
    const newPlant: Plant= new Plant(
      "0",
      this.addForm.value.name!,
      this.addForm.value.description!,
      this.addForm.value.maintenance!,
      this.addForm.value.imageUrl!,
      this.addForm.value.userId!);

      if (newPlant == null){
        //snackbar -> unsuccesful add
      }else{
       /* this.webApi.addPlant(newPlant).subscribe({
          next: (plant) => {this.plants.push(plant), this.updateList.emit(this.plants)},
          error: (error) => {console.log('Adding failed', error)}
        });*/
      }
      console.log(newPlant);
  }

}
