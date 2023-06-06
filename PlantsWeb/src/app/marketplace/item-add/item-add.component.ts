import { formatDate } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { Item } from 'src/app/models/Item';
import { ItemType } from 'src/app/models/ItemType';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { UserService } from 'src/app/user.service';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-item-add',
  templateUrl: './item-add.component.html',
  styleUrls: ['./item-add.component.css']
})
export class ItemAddComponent {

  items: Item[] = [];
  currentUserId! : string;

  fileName = '';

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
    private data: DataService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private userService: UserService
    ) 
    {this.currentUserId = userService.LoggedInUser()}
    

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
      userId: this.currentUserId,
      user: null
    };
    console.log(newItem);

    this.webApi.addItem(newItem).subscribe({
      next: (res) => {
        //this.items.push(res),
       // this.newMessage(this.items),
        this.goToMarketPlace();
        this.openSnackBar("Successfully added item for sale!"); },
      error: (err) => {
        this.openSnackBar("Couldn't add item. Try again!"),
        console.log(err)}
    })
    
    
  }

 /* newMessage(updatedItems : ItemDto[]) {
    this.data.changeItemsMessage(updatedItems);
  }*/

  goToMarketPlace(){
    this.router.navigate(['/marketplace']);
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

  onFileChanged(event : any ){
    
    const file:File = event.target.files[0];

    if (file) {

        this.fileName = file.name;

        const formData = new FormData();

        formData.append("thumbnail", file);

        //const upload$ = this.http.post("/api/thumbnail-upload", formData);

        
    }
  }
  cancelUpload() {
    
    this.reset();
  }

  reset() {
    this.fileName = '';
  }
}
