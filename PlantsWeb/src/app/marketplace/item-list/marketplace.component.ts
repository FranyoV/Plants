import {AfterViewInit, Component, OnChanges, OnDestroy, OnInit, ViewChild} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { ItemDetailsComponent } from '../item-details/item-details.component';
import { Item } from 'src/app/models/Item';
import { ActivatedRoute, Router } from '@angular/router';
import { WebApiService } from 'src/app/webapi.service';
import { DataService } from 'src/app/data.service';
import { Subscription } from 'rxjs';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ItemDto } from 'src/app/models/ItemDto';


@Component({
  selector: 'app-marketplace',
  templateUrl: './marketplace.component.html',
  styleUrls: ['./marketplace.component.css']
})
export class MarketplaceComponent  implements OnInit, AfterViewInit, OnDestroy {
  displayedColumns: string[] = ['name', 'type', 'price', 'username', 'date', 'details'];
  myDisplayedColumns: string[] = ['name', 'type', 'price', 'date', 'edit', 'delete'];
  dataSource!: MatTableDataSource<ItemDto>;
  dataSourceMyItems!: MatTableDataSource<Item>;
  animal: string = '';
  name: string = '';
  items: ItemDto[] = [];
  myItems: Item[] = [];
  subscription!: Subscription;
  currentUserId!: string;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    public dialog: MatDialog,
    private router: Router,
    private webApi: WebApiService,
    private data : DataService,
    private route : ActivatedRoute,
    private snackBar : MatSnackBar
    ) 
    {}

    
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }


  ngOnInit(): void {
    this.subscription = this.data.currentItemsMessage
    .subscribe( message => this.items = message ) ;

   this.route.parent?.params.subscribe({
      next: (params) => {
        const id = params["userId"];
        this.currentUserId = id!;
      },
      error: (err) => this.openSnackBar("Something went wrong!")
    });

    this.getItems();
    this.getMyItems();
      
  }

  getItems(){
    this.webApi.getItems().subscribe({
      next: (res) => {this.items = res, console.log(res),
        this.dataSource = new MatTableDataSource(this.items);},
      error: (err) => {console.error(err)}
    })
  }

  getMyItems(){
    this.webApi.getItemsOfUser(this.currentUserId).subscribe({
      next: (res) => { this.dataSourceMyItems = new MatTableDataSource(res)},
      error: (err) => { console.error(err)}
    })
  }



  openDialog(item: ItemDto): void {

    const dialogRef = this.dialog.open(ItemDetailsComponent, {
      data: {
        name: item.name, 
        description: item.description, 
        type: item.type,
        date: item.date,
        price: item.price,
        username: item.username,
        email: item.email
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      
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


  deleteItem(item: Item){
    this.webApi.deleteItem(item.id).subscribe({
      next: (res) => {
        this.openSnackBar("Item successfully deleted!");
        const index = this.myItems.findIndex(i => i.id == res.id);
        this.myItems.splice(index, 1);
        this.dataSourceMyItems._renderChangesSubscription;
      },
      error: (err) => {this.openSnackBar("Couldn't delete item!")}
    })
  }

  goToAddItemPage(){
    this.router.navigate([`${this.currentUserId}/item/new`]);
  }

  goToEditItemPage(item: Item){
    this.router.navigate([`${this.currentUserId}/items/${item.id}`]);
  }
  
  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

}