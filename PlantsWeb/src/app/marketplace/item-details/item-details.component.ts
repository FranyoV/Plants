import { Component, Inject } from '@angular/core';
import {MatDialog, MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { Item } from 'src/app/models/Item';
import { ItemDto } from 'src/app/models/ItemDto';
import { ItemType } from 'src/app/models/ItemType';


@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent {

  enumDict!: ItemType;

  constructor(
    public dialogRef: MatDialogRef<ItemDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ItemDto,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
