import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Post } from 'src/app/models/Post';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { UserService } from 'src/app/user.service';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.css']
})
export class PostEditComponent {
  post!: Post;
  currentUserId: string = "";
  currentPostId: string = "";
  posts: Post[] = [];
  
  fileName = '';
  file! : File;
  formData!: FormData;

  editPostForm = this.formBuilder.group({
    title : ['', [Validators.required, Validators.maxLength(50)]],
    content : ['', [Validators.required]],
    imageData: [''],
  })

  get title() {
    return this.editPostForm.get('title');
  }

  get content() {
    return this.editPostForm.get('content');
  }

  
  subscription! : Subscription;

  constructor(
    private formBuilder: FormBuilder,
    private webApi: WebApiService,
    private data: DataService,
    private router: Router,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private userService : UserService
    ){
      this.currentUserId = this.userService.LoggedInUser();
    }


  ngOnInit(): void {
    this.data.currentPostsMessage.subscribe( message => this.posts = message );

    this.route.paramMap.subscribe( (params) => {
      console.log(params)
      const id = params.get("postId");
      this.currentPostId = id!;
    console.log(this.currentPostId)
    })


    this.webApi.getPostById(this.currentPostId).subscribe({
      next: (res) => { this.post = res,
        this.editPostForm.controls['title'].setValue(this.post.title!);
        this.editPostForm.controls['content'].setValue(this.post.content!); },
      error: (err) => { console.error("Coutldn't get posts.", err)}
    })      
  }
  


  editPost(){
    const newPost: Post = new Post(
      this.currentPostId,
      this.editPostForm.value.title!,
      this.editPostForm.value.content!,
      this.post.imageData!,
      this.post.dateOfCreation,
      this.currentUserId,
      null,
      null,
      null
    )

    if ( newPost != null ){
      this.webApi.updatePost(this.currentPostId, newPost).subscribe({
        next: (res) => { 
          if (this.fileName.length > 0){
            this.addImage(res);
          }
          else{
            let index = this.posts.findIndex(p => p.id = res.id);
            this.posts.splice(index, 1, res);
            this.newMessage(this.posts), 
            this.router.navigate([`main`]);
            this.openSnackBar("Post successfully edited!"); 
          }
        },
        error: (err) => { 
          this.openSnackBar("Something went wrong!"), 
          console.error("Failed edit post.", err)}
      })
    }
  }

  onFileChanged(event : any ){
    const file:File = event.target.files[0];

    if (file) {
        this.fileName = file.name;
        this.formData = new FormData();
        this.formData.append("image", file);
        this.file = file;
        
        const images = event.target.files;
        if (images.length === 0)
            return;
    }
  }
  cancelUpload() {
    this.fileName = '';
  }


  addImage(post : Post){
    this.webApi.addImageToPost(this.formData, this.currentPostId).subscribe({
      next: (res) => {
        let index = this.posts.findIndex(p => p.id = post.id);
        this.posts.splice(index, 1, res);
        
        this.newMessage(this.posts), 
        this.router.navigate(['main'])
        this.openSnackBar("Post successfully edited!")
      },
      error: (err) => {this.openSnackBar("Image upload was unsuccessful. Try again.")}
    })
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


  goBack(){
    this.router.navigate([`main`]);
  }
}

