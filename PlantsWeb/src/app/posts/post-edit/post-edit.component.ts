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
  fileName = '';

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
      "00000000-0000-0000-0000-000000000000",
      this.editPostForm.value.title!,
      this.editPostForm.value.content!,
      this.editPostForm.value.imageData!,
      new Date(),
      this.currentUserId,
      null,
      null,
      null
    )

    if ( newPost != null ){

      this.webApi.updatePost(newPost.id, newPost).subscribe({
        next: (res) => { 
           this.router.navigate([`${this.currentUserId}/main`]);
           this.openSnackBar("New post created!"); },
        error: (err) => { 
          this.openSnackBar("Something went wrong!"), 
          console.error("Failed to create the new post", err)}
      })
    }
  }

  onFileChanged(event : any ){
    
    const file:File = event.target.files[0];

    if (file) {

        this.fileName = file.name;

        const formData = new FormData();

        formData.append("thumbnail", file);

        //const upload$ = this.http.post("/api/thumbnail-upload", formData);

        
    }
  }
  cancelUpload() {
    
    this.reset();
  }

  reset() {
    this.fileName = '';
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

