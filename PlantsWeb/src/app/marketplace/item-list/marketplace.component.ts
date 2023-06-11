import {AfterViewInit, Component, OnChanges, OnDestroy, OnInit, ViewChild} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort, Sort} from '@angular/material/sort';
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
import { UserService } from 'src/app/user.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ItemType } from 'src/app/models/ItemType';
import { LiveAnnouncer } from '@angular/cdk/a11y';

@Component({
  selector: 'app-marketplace',
  templateUrl: './marketplace.component.html',
  styleUrls: ['./marketplace.component.css']
})
export class MarketplaceComponent  implements OnInit, AfterViewInit, OnDestroy {

  displayedColumns: string[] = ['name', 'price', 'username', 'date', 'details'];
  myDisplayedColumns: string[] = ['name', 'price', 'date', 'edit', 'delete'];
  dataSource: MatTableDataSource<ItemDto> = new MatTableDataSource<ItemDto>();
  dataSourceMyItems: MatTableDataSource<Item> = new MatTableDataSource<Item>();
  animal: string = '';
  name: string = '';
  items: ItemDto[] = [];
  myItems: Item[] = [];
  subscription!: Subscription;
  currentUserId!: string;


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort : MatSort = new MatSort();


  constructor(
    public dialog: MatDialog,
    private router: Router,
    private webApi: WebApiService,
    private data : DataService,
    private route : ActivatedRoute,
    private snackBar : MatSnackBar,
    private userService: UserService,
    private sanitizer: DomSanitizer,
    private _liveAnnouncer: LiveAnnouncer
    ) 
    {this.currentUserId = userService.LoggedInUser()}

    
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }


  ngOnInit(): void {
    this.subscription = this.data.currentItemsMessage
    .subscribe( message => this.items = message ) ; 
  }

  ngAfterViewInit() {
    this.webApi.getItems().subscribe({
      next: (res) => {
        this.items = this.convertImages(res),
        this.dataSource = new MatTableDataSource<ItemDto>(this.items);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;},
      error: (err) => {console.error(err)}
    })
    
    this.webApi.getItemsOfUser(this.currentUserId).subscribe({
      next: (res) => { 
        this.dataSourceMyItems = new MatTableDataSource<Item>(res);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;;},
      error: (err) => { console.error(err)}
    })


  }


  convertImages(items : ItemDto[]): ItemDto[]{
    items.forEach(item => {
      if (item.imageData != undefined){
        let objectURL = 'data:image/png;base64,' + item.imageData;
        item.imageData = this.sanitizer.bypassSecurityTrustUrl(objectURL);
      }
    });
    return items;
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
        email: item.email,
        imageData: item.imageData
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      
    });
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
        console.log(index)
        this.myItems.splice(index, 1);
        this.dataSource._updateChangeSubscription();
        this.ngAfterViewInit();
        this.dataSourceMyItems._updateChangeSubscription();
        this.ngAfterViewInit();
      },
      error: (err) => {this.openSnackBar("Couldn't delete item!")}
    })
  }

  goToAddItemPage(){
    this.router.navigate([`item/new`]);
  }

  goToEditItemPage(item: Item){
    this.router.navigate([`items/${item.id}`]);
  }
  
  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

}