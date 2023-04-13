import { Component, OnInit } from '@angular/core';
import { User } from '../models/User';
import { WebApiService } from '../webapi.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  currentUserId!: string;
  currentUser!: User | undefined;

  constructor(
    private webApi: WebApiService,
    private route: ActivatedRoute
  ){}

  ngOnInit(): void {
    this.route.paramMap.subscribe( (params) => {
      const id = params.get("userId");
      this.currentUserId = id!;
    } )

    this.webApi.getUserById(this.currentUserId).subscribe({
      next: (res) => { this.currentUser = res},
      error: (error) => {console.log("No user with this id.", error)}
    })
  }

}
