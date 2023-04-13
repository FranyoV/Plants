import { Component, OnInit } from '@angular/core';
import { WebApiService } from '../../webapi.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../models/Post';
import { Reply } from 'src/app/models/Reply';
import { FormBuilder, FormControl, Validators } from '@angular/forms';


@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.css']
})
export class PostDetailsComponent implements OnInit {

  currentPost: Post | undefined ;
  currentPostId: string = "";
  replies: Reply[] = [];
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
    private formBuilder: FormBuilder
  ){}

  ngOnInit(): void {
    this.route.paramMap.subscribe( (params) => {
      const id = params.get("postId");
      this.currentPostId = id!;
    } )

    

    this.webApi.getPostById(this.currentPostId).subscribe({
      next: (result) => { this.currentPost = result},
      error: (error) => { console.error("No matching posts found for this id.", error)}
    });

    this.webApi.getRepliesOfPost(this.currentPostId).subscribe({
      next: (res) => { this.replies = res},
      error: (error) => { console.log( "NoReplies for this post.", error)}
      
    })
    
  }

  addReply(){
    var newReply = new Reply(
      "00000000-0000-0000-0000-000000000000",
      this.addReplyForm.value.content!,
      new Date(),
      this.currentPostId,
      "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    )
    //this.replies.push(newReply);

    this.webApi.addReplies(newReply).subscribe({
      next: (res) => { this.replies.push(res) },
      error: (error ) => {console.log("Adding comment failed", error)}
      
    })
  }

}