import { Component, OnInit } from '@angular/core';
import { WebApiService } from '../../webapi.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../models/Post';
import { Reply } from 'src/app/models/Reply';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { SnackbarComponent } from 'src/app/snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReplyDto } from 'src/app/models/ReplyDto';
import { UserService } from 'src/app/user.service';


@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.css']
})
export class PostDetailsComponent implements OnInit {

  currentUserId!: string;
  currentPost: Post | undefined ;
  currentPostId: string = "";
  replies: ReplyDto[] = [];
  reply: string = "";

   addReplyForm  = this.formBuilder.group({
    content : ['', [Validators.required]]
  })

  get content(){
    return this.addReplyForm.get('content');
  }


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private webApi: WebApiService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private userService : UserService
  ){
    this.currentUserId = this.userService.LoggedInUser();
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe( (params) => {
      const id = params.get("postId");
      this.currentPostId = id!;
    } )

    this.getPostById();
    this.getRepliesOfPost();
  }

  getPostById(){
    this.webApi.getPostById(this.currentPostId).subscribe({
      next: (result) => { this.currentPost = result },
      error: (error) => { this.openSnackBar("Something went wrong. Try again later!"), console.error("No matching posts found for this id.", error)}
    });
  }

  getRepliesOfPost(){
    this.webApi.getRepliesOfPost(this.currentPostId).subscribe({
      next: (res) => { this.replies = res},
      error: (error) => { this.openSnackBar("Something went wrong. Try again later!"), console.log( "No replies for this post.", error)}
    })
  }

  addReply(){
    var newReply = new Reply(
      "00000000-0000-0000-0000-000000000000",
      this.addReplyForm.value.content!,
      new Date(),
      this.currentPostId,
      this.currentUserId
    )

    this.webApi.addReplies(newReply).subscribe({
      next: (res) => { 
        
        this.replies.push(res) ,
        this.openSnackBar("Reply successfully added!")
        this.addReplyForm.controls['content'].setValue('');
        this.addReplyForm.controls['content'].markAsUntouched();
        //sthis.addReplyForm.controls['content'].setValue('');
      },
      error: (error ) => {
        this.openSnackBar("Couldn't add reply. Try again!"),
        console.log("Adding comment failed", error)}
      
    })
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }


  goBack(){
    this.router.navigate([`${this.currentUserId}/main`])
  }

  deleteReply(replyId : string){
      console.log("delete clicked");
  }
}