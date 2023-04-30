import { Component, OnInit } from '@angular/core';
import { WebApiService } from '../webapi.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbar/snackbar.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{

  currentUserId!: string ;

  constructor(
    private webApi : WebApiService ,
    private snackBar : MatSnackBar 
  ){}

  ngOnInit(): void {
    this.webApi.getMe().subscribe({
      next: (res) => {
        this.currentUserId = res, console.log("You are logged in with user: ",this.currentUserId);
      },
      error: (err) => {this.openSnackBar("Something went wrong. Try again!"), console.error('Getting plant for user failed.',err)}
      })
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
}
