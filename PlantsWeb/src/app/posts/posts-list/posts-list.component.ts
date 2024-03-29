import { Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../models/Post';
import { WebApiService } from '../../webapi.service';
import { PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';

import { UserService } from 'src/app/user.service';

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
    private snackBar: MatSnackBar,
    private route : ActivatedRoute,
    private userService : UserService
  ){
    this.currentUserId = this.userService.LoggedInUser();
  }

  ngOnInit() : void {
  
    this.subscription = this.data.currentPostsMessage
    .subscribe( message => this.posts = message ) ;
   
    this.getPosts();
    this.getPostByUserReplies();
    this.getPostByUser();
  
  }



  getPostByUser(){
    console.log(this.currentUserId)
    this.webApi.getPostByUser(this.currentUserId).subscribe({
      next: (res) => {this.postsOfUser = res},
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
    })
  }

  getPostByUserReplies(){
    console.log(this.currentUserId)
    this.webApi.getPostByUserReplies(this.currentUserId).subscribe({
      next: (res) => {this.postsOfUserReplies = res},
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
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

  deletePost(postId: string){
    this.webApi.deletePost(postId).subscribe({
      next: (res) => {
        if (!res){
          this.openSnackBar("Deleting post was unsuccesfull. ");
        }
        else
        {this.openSnackBar("Post succesfully deleted. ");
        let index = this.posts.findIndex(p => p.id == postId);
        this.posts.splice(index, 1);
        index = this.postsOfUser.findIndex(p => p.id == postId);
        this.postsOfUser.splice(index, 1);
        index = this.postsOfUserReplies.findIndex(p => p.id == postId);
        this.postsOfUserReplies.splice(index, 1)}
      },
      error: (err) => {this.openSnackBar("Deleting post was unsuccesfull. ")
        
      },
    })
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
  
  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
