import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Post } from 'src/app/models/Post';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-post-add',
  templateUrl: './post-add.component.html',
  styleUrls: ['./post-add.component.css']
})
export class PostAddComponent implements OnInit{

  posts: Post[] = [];
  currentUserId: string = "";

  addPostForm = this.formBuilder.group({
    title : ['', [Validators.required, Validators.maxLength(50)]],
    content : ['', [Validators.required]],
    imageUrl: [''],
  })

  get title() {
    return this.addPostForm.get('title');
  }

  get content() {
    return this.addPostForm.get('content');
  }

  
  subscription! : Subscription;

  constructor(
    private formBuilder: FormBuilder,
    private webApi: WebApiService,
    private data: DataService,
    private router: Router,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute
    ){}


  ngOnInit(): void {
    /*this.webApi.getMe().subscribe({
      next: (res) => {
        this.currentUserId = res, console.log("You are logged in with user: ",this.currentUserId);

      },
      error: (err) => {this.openSnackBar("something went wrong. Try again later!")
      },

    })*/

    this.route.parent?.params.subscribe({
      next: (params) => {
        const id = params["userId"];
        this.currentUserId = id!;
      },
      error: (err) => this.openSnackBar("Something went wrong!")
    });

    this.webApi.getPosts().subscribe({
      next: (res) => { this.posts = res },
      error: (err) => { console.error("Coutldn't get posts.", err)}
    })
  }


  addPost(){
    const newPost: Post = new Post(
      "00000000-0000-0000-0000-000000000000",
      this.addPostForm.value.title!,
      this.addPostForm.value.content!,
      this.addPostForm.value.imageUrl!,
      new Date(),
      this.currentUserId,
      null
    )

    if ( newPost == null ){
      //snackbar
    }
    else{
      this.webApi.addPost(newPost).subscribe({
        next: (res) => { 
           this.posts.push(res),
           this.newMessage(this.posts),
           this.router.navigate(['main'])
           this.openSnackBar("New post created!"); },
        error: (err) => { 
          this.openSnackBar("Something went wrong!"), 
          console.error("Failed to create the new post", err)}
      })
    }
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
  
  newMessage(updatedPosts : Post[]) {
    this.data.changePostsMessage(updatedPosts);
  }

}
