import { Component, OnInit } from '@angular/core';
import { WebApiService } from '../webapi.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbar/snackbar.component';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{

  currentUserId!: string ;

  constructor(
    private webApi : WebApiService ,
    private snackBar : MatSnackBar ,
    private route : ActivatedRoute,
    private userService : UserService
  ){this.currentUserId = userService.LoggedInUser();}

  ngOnInit(): void {
      console.log("Your currently logged in with userId: ",this.currentUserId);
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
}
