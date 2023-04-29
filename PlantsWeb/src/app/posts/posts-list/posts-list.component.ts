import { Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '../../models/Post';
import { WebApiService } from '../../webapi.service';
import { PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';

@Component({
  selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.css']
})
export class PostsListComponent implements OnInit, OnDestroy{

  @Input() currentUserId!: string; 
  posts : Post[] = [];
  postsOfUser : Post[] = [];
  postsOfUserReplies : Post[] = [];
  searchText!: string;
  subscription!: Subscription;
  pageSlice = this.posts.slice(0, 5);

  durationInSeconds = 5;

  constructor(
    private router : Router,
    private webApi : WebApiService,
    private data : DataService,
    private snackBar: MatSnackBar
  ){}

  ngOnInit() : void {
    this.webApi.getMe().subscribe({
      next: (res) => {
        this.currentUserId = res, console.log("You are logged in with user: ",this.currentUserId);
        this.getPostByUserReplies();
        this.getPostByUser();
    }

    })
    
    this.data.currentUserIdMessage.subscribe({
      next: (res) => {this.currentUserId = res, console.log("You are logged in with user: ",this.currentUserId)}
    })
    this.subscription = this.data.currentPostsMessage
    .subscribe( message => this.posts = message ) ;

    this.getPosts();
    this.getReplyCount();


  
  }

  getPostByUser(){
    this.webApi.getPostByUser(this.currentUserId).subscribe({
      next: (res) => {this.postsOfUser = res},
      error: (err) => {console.log(err)}
    })
  }

  getPostByUserReplies(){
    this.webApi.getPostByUserReplies(this.currentUserId).subscribe({
      next: (res) => {this.postsOfUserReplies = res},
      error: (err) => {console.log(err)}
    })
  }


  getReplyCount(){
    console.log("im in")
    this.posts.forEach(function () {
      console.log("meh")
    });
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
