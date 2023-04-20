import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '../../models/Post';
import { WebApiService } from '../../webapi.service';
import { PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';

@Component({
  selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.css']
})
export class PostsListComponent implements OnInit, OnDestroy{

  posts : Post[] = [];
  searchText!: string;
  subscription!: Subscription;
  pageSlice = this.posts.slice(0, 5);

  constructor(
    private router : Router,
    private webApi : WebApiService,
    private data : DataService,
  ){}

  ngOnInit() : void {
    this.subscription = this.data.currentPostsMessage
    .subscribe( message => this.posts = message ) ;

    this.getPosts();
  }

  getPosts(){
    this.webApi.getPosts().subscribe({
      next: (result) => { this.posts = result },
      error: (error) => {console.error("Can't get posts from api.", error)}
    })
  }


  goToPostDetails(postId: string){
    this.router.navigate([`post/${postId}`]);
  }

  goToAddPostPage(){
    this.router.navigate([`posts/new`]);
  }


  //PAGINATOR EVENTHANDLER
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

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
