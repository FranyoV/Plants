import { Component, OnInit } from '@angular/core';
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
  selector: 'app-item-edit',
  templateUrl: './item-edit.component.html',
  styleUrls: ['./item-edit.component.css']
})
export class ItemEditComponent implements OnInit{

  editForm = this.formBuilder.group({
    name : ['', [Validators.required, Validators.maxLength(50)]],
    type: ['', [Validators.required]] ,
    price: [0, [Validators.required, Validators.minLength(1)]],
    description : [''],
    image: [''],
    sold: [true, [Validators.required]]
  })


  items: ItemDto[] = [];
  currentItemId: string = '';
  currentItem!: Item;
  currentUserId: string = '';

  fileName: string = "";
  formData!: FormData ;
  file!: File;

  constructor(
    private formBuilder: FormBuilder,
    private webApi: WebApiService,
    private router: Router,
    private data: DataService,
    private route: ActivatedRoute,
    private userService: UserService,
    private snackBar: MatSnackBar,
    ) 
    {this.currentUserId = userService.LoggedInUser()}
    
    ngOnInit(): void {
   // this.data.currentItemsMessage.subscribe( message => this.items = message );

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("itemId");
      this.currentItemId = id!;})


      if(this.currentItem == null){
        this.webApi.getItemsById(this.currentItemId).subscribe({
          next: (result) => {
             this.currentItem = result; console.log("currentplant:", this.currentItem);
             this.editForm.controls['name'].setValue(this.currentItem.name);
             this.editForm.controls['type'].setValue("ItemType.BOWL");
             this.editForm.controls['description'].setValue(this.currentItem.description);
             this.editForm.controls['image'].setValue(this.currentItem.imageUrl );
             this.editForm.controls['price'].setValue(this.currentItem.price );},
             
          error: (error) => { console.error("No plants with this id.", error)}
        });
      }
  }

  editItem(){
    
    const modifiedItem : Item= {
      id: this.currentItemId,
      name: this.editForm.value.name!,
      description: this.editForm.value.description!,
      type: ItemType.BOWL,
      price: this.editForm.value.price!,
      date: this.currentItem.date,
      userId: this.currentItem.userId,
      user: null,
      imageUrl: null,
      sold: false
    }
     

      if (modifiedItem != null){
       
        this.webApi.editItem(this.currentItemId, modifiedItem).subscribe({
          next: (result) => {  
            if (this.fileName.length > 0 || this.formData != undefined){
              this.addImage(this.formData, this.currentItemId);
            }
            else{
              let index = this.items.findIndex(p => p.id = result.id);
              this.items.splice(index, 1, result);
              this.newMessage(this.items)
            }
            

          this.router.navigate(['marketplace']);},
            
          error: (error) => {this.openSnackBar('Adding failed. Try again.')}
        });
      }
  }


  onFileChanged(event : any ){
    const file:File = event.target.files[0];

    if (file) {
        this.fileName = file.name;
        this.formData = new FormData();

        this.formData.append("image", file);
        console.log(this.formData.get("image"));
        this.file = file;
        const images = event.target.files;
        if (images.length === 0)
            return;
    }
  }

  cancelUpload() {
    this.fileName = '';
  }


  addImage(image: FormData, itemId: string){
    
    this.webApi.addImageToItem(image, itemId).subscribe({
      next: (res) => {this.openSnackBar("Item successfully edited.");
      let index = this.items.findIndex(p => p.id = res.id);
      this.items.splice(index, 1, res);
      this.newMessage(this.items)
     },
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
    })
  }

  newMessage(updatedItems : ItemDto[]) {
    this.data.changeItemsMessage(updatedItems);
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

  goToMarketPlace(){
    this.router.navigate(['/marketplace']);
  }
}
