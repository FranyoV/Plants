import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '../models/Post';
import { WebApiService } from '../webapi.service';
import { Guid } from 'guid-typescript';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit{

  posts : Post[] = [];
  searchText!: string;
  pageSlice = this.posts.slice(0, 5);

  constructor(
    private router: Router,
    private webApi: WebApiService
  ){}

  ngOnInit() : void {

    this.getPosts();
  }

  getPosts(){
    this.webApi.getPosts().subscribe({
      next: (result) => {
        
        this.posts = result,
        console.log("before",this.posts);
        //this.getUsers(),
       // console.log("after", this.posts);
      },
      error: (error) => {console.error("Can't get posts from api.", error)}
    })
  }

  async getUsers(){
    for ( let post of this.posts){
      (await this.webApi.getUserById(post.userId)).subscribe({
        next: (result) => { post.user = result },
        error: (error) => { console.error("no user found for this post", error)}
       })
    }
  }


  goToPostDetails(postId: Guid){
    this.router.navigate([`post/${postId}`]);
  }

  goToAddPostPage(){
    this.router.navigate([``]);
  }


  OnPageEvent(event: PageEvent){
      console.log(event);
      const startIndex = event.pageIndex * event.pageSize;
      let endIndex = startIndex + event.pageSize;

      if (endIndex > this.posts.length) {
        endIndex = this.posts.length;
      }
      this.pageSlice = this.posts.slice(startIndex, endIndex);

  }
  //this.router.navigate( [`${this.currentUserIdNumber}/details`] )

}
