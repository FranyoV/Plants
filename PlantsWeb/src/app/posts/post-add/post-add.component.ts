import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/data.service';
import { Post } from 'src/app/models/Post';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { UserService } from 'src/app/user.service';
import { WebApiService } from 'src/app/webapi.service';

@Component({
  selector: 'app-post-add',
  templateUrl: './post-add.component.html',
  styleUrls: ['./post-add.component.css']
})
export class PostAddComponent implements OnInit{

  posts: Post[] = [];
  currentUserId: string = "";

  fileName = '';

  addPostForm = this.formBuilder.group({
    title : ['', [Validators.required, Validators.maxLength(50)]],
    content : ['', [Validators.required]],
    imageUrl: [''],
  })
  formData: FormData | null = null;
  file!: File;

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
    private route: ActivatedRoute,
    private userService : UserService
    ){
      this.currentUserId = this.userService.LoggedInUser();
    }


  ngOnInit(): void {
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
      null,
      null,
      null
    )

    if ( newPost == null ){
      //snackbar
    }
    else{
      this.webApi.addPost(newPost).subscribe({
        next: (res) => { 
          if (this.fileName.length > 0){
            this.webApi.addImageToPost(this.formData!, res.id).subscribe({
              next: () => {this.openSnackBar("Image uploaded.");
              this.posts.push(res),
              this.newMessage(this.posts),
              this.router.navigate([`main`]);
              this.openSnackBar("New post created!");},
              error: () => {this.openSnackBar("Unsuccessful image upload")}
             })

             }else{
              this.posts.push(res),
             this.newMessage(this.posts),
             this.router.navigate([`main`]);
             this.openSnackBar("New post created!");
             }
            },
          
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
