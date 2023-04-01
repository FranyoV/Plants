import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Maintenance } from 'src/app/models/Maintenance';

@Component({
  selector: 'app-plants-edit',
  templateUrl: './plants-edit.component.html',
  styleUrls: ['./plants-edit.component.css']
})
export class PlantsEditComponent {
  editForm = this.formBuilder.group({
    id : "",
    name : "",
    description : "",
    maintenance: new Maintenance(),
    imageUrl: "" ,
    userId: ""
  })


  constructor(
    private formBuilder: FormBuilder
  ){  }

  update(){
    
  }
}
