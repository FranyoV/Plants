import { Component } from '@angular/core';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbModal, NgbNavbar, NgbPanelToggle } from '@ng-bootstrap/ng-bootstrap'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {
  profilenavlink: string = "/profile";
  constructor(
    private router: Router
  ){}

  //this.router.navigate( [`${this.currentUserIdNumber}/details`] )

}
