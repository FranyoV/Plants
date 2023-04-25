import {AfterViewInit, Component, OnChanges, OnDestroy, OnInit, ViewChild} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { ItemDetailsComponent } from '../item-details/item-details.component';
import { Item } from 'src/app/models/Item';
import { ItemType } from 'src/app/models/ItemType';
import { Router } from '@angular/router';
import { WebApiService } from 'src/app/webapi.service';
import { DataService } from 'src/app/data.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-marketplace',
  templateUrl: './marketplace.component.html',
  styleUrls: ['./marketplace.component.css']
})
export class MarketplaceComponent  implements OnInit, AfterViewInit, OnDestroy {
  displayedColumns: string[] = ['name', 'type', 'price', 'username', 'date', 'details'];
  dataSource!: MatTableDataSource<Item>;
  animal: string = '';
  name: string = '';
  items: Item[] = [];
  myItems: Item[] = [];
  subscription!: Subscription;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    public dialog: MatDialog,
    private router: Router,
    private webApi: WebApiService,
    private data : DataService,
    ) {


  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }


  ngOnInit(): void {
    this.subscription = this.data.currentItemsMessage
    .subscribe( message => this.items = message ) ;

    this.getItems();
      
  }

  getItems(){
    this.webApi.getItems().subscribe({
      next: (res) => {this.items = res, console.log(res),
        this.dataSource = new MatTableDataSource(this.items);},
      error: (err) => {console.error(err)}
    })
  }

  getMyItems(){
    this.webApi.getItemsOfUser("3fa85f64-5717-4562-b3fc-2c963f66afa6").subscribe({
      next: (res) => { this.myItems = res},
      error: (err) => { console.error(err)}
    })
  }



  openDialog(): void {

    const dialogRef = this.dialog.open(ItemDetailsComponent, {
      data: {name: this.name, animal: this.animal},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.animal = result;
    });
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  goToAddItemPage(){
    this.router.navigate(['marketplace/new']);
  }
  

}