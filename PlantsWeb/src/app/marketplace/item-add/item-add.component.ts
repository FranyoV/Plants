import { formatDate } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { Item } from 'src/app/models/Item';
import { ItemDto } from 'src/app/models/ItemDto';
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

  items: ItemDto[] = [];
  currentUserId! : string;

  addForm = this.formBuilder.group({
    name : ['', [Validators.required, Validators.maxLength(50)]],
    type: ['', [Validators.required]] ,
    price: [0, [Validators.required, Validators.minLength(1)]],
    description : [''],
    image: ['']
  })
  
  fileName: string = "";
  formData!: FormData ;
  file!: File;

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
        if (this.fileName.length > 0 || this.formData != undefined || this.formData != null){
          this.addImage(this.formData, res.id);
        }else{
          this.items.push(res),
          this.newMessage(this.items)
        }

        this.goToMarketPlace();
        this.openSnackBar("Successfully added item for sale!"); },
      error: (err) => {
        this.openSnackBar("Couldn't add item. Try again!"),
        console.log(err)}
    })
    
    
  }
  currentItemId(formData: any, currentItemId: any) {
    throw new Error('Method not implemented.');
  }

  newMessage(updatedItems : ItemDto[]) {
    this.data.changeItemsMessage(updatedItems);
  }

  addImage(image: FormData, itemId: string){
    console.log("adding image to item")
    this.webApi.addImageToItem(image, itemId).subscribe({
      next: (res) => {console.log("succesful image upload"),         
      this.items.push(res),
      this.newMessage(this.items)},
      error: (err) => {console.error(err)}
    })
  }

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
        this.formData = new FormData();

        this.formData.append("image", file);
        console.log(this.formData.get("image"));
        this.file = file;
        const files = event.target.files;
        if (files.length === 0)
            return;
    }
  }
  cancelUpload() {
    
    this.reset();
  }

  reset() {
    this.fileName = '';
  }
}
