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
    /*this.webApi.getMe().subscribe({
      next: (res) => {
        this.currentUserId = res, console.log("You are logged in with user: ",this.currentUserId);
      },
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
      })*/
    /*  this.route.paramMap.subscribe( (params) => {
        const id = params.get("userId");
        this.currentUserId = id!;

      } )*/
      console.log(this.currentUserId);
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
}
