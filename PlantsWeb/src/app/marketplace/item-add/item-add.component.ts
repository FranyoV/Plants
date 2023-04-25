import { formatDate } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { Item } from 'src/app/models/Item';
import { ItemType } from 'src/app/models/ItemType';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-item-add',
  templateUrl: './item-add.component.html',
  styleUrls: ['./item-add.component.css']
})
export class ItemAddComponent {
  items: Item[] = [];

  addForm = this.formBuilder.group({
    name : ['', [Validators.required, Validators.maxLength(50)]],
    type: ['', [Validators.required]] ,
    price: [0, [Validators.required, Validators.minLength(1)]],
    description : [''],
    image: ['']
  })

  constructor(
    private formBuilder: FormBuilder,
    private webApi: WebApiService,
    private router: Router,
    private data: DataService
    ){}

  addItem(){

    const newItem: Item = {
      id: "00000000-0000-0000-0000-000000000000",
      name: this.addForm.value.name!,
      type: ItemType.BOWL,
      price: Number(this.addForm.value.price!),
      description: this.addForm.value.description!,
      imageUrl: this.addForm.value.image!,
      date: new Date(),
      sold: false,
      userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      user: null
    };


    this.webApi.addItem(newItem).subscribe({
      next: (res) => {
        this.items.push(res),
        this.newMessage(this.items),
        this.router.navigate(['marketplace']); },
      error: (err) => {console.log(err)}
    })
    
    
  }

  newMessage(updatedItems : Item[]) {
    this.data.changeItemsMessage(updatedItems);
  }

  goToMarketPlace(){
    this.router.navigate(['/marketplace']);
  }

}
