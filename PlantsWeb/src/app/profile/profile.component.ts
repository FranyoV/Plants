import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { User } from '../models/User';
import { WebApiService } from '../webapi.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { FormBuilder, Validators } from '@angular/forms';
import { SnackbarComponent } from '../snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserInfoEditRequest } from '../models/UserInfoEditRequest';
import { UserService } from '../user.service';
import { UserDto } from '../models/UserDto';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

Chart.register(...registerables);


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnChanges {

  currentUserId!: string;
  currentUser: UserDto = new UserDto ;
  repliesCount: number = 1;
  postsCount: number = 1;
  plantCount: number = 1;
  chart!: Chart;

  changeEmailForm = this.formBuilder.group({
    email : ['', [ Validators.required, Validators.email]],
    imageUrl: '' ,
    password: [ '', [ Validators.required]]
  })

  changePasswordForm = this.formBuilder.group({
    newPassword : ['',
    [ Validators.required, 
      Validators.minLength(8), 
      Validators.maxLength(12),
      Validators.pattern('^[a-zA-Z]+$')]],
    currentPassword: [ '', 
    [ Validators.required, 
      Validators.minLength(8), 
      Validators.maxLength(12),
      Validators.pattern('^[a-zA-Z]+$')]]
  })

  fileName: string = "";
  formData!: FormData ;
  file!: File;

  constructor(
    private webApi: WebApiService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router,
    private userService : UserService,
    private sanitizer: DomSanitizer
  ){
    this.currentUserId = userService.LoggedInUser();
  }

  ngOnInit(): void {
    this.getUserById();
    this.getReplyCount();
    this.getPlantCount();
    this.getPlantCount();
  }


  getUserById(){
    this.webApi.getUserById(this.currentUserId).subscribe({
      next: (res) => {
        console.log("res.imagedata: ", res.imageData);
        this.currentUser = res;
        if (res.imageData != undefined){
          let objectURL = 'data:image/png;base64,' + res.imageData;
          this.currentUser.imageData = this.sanitizer.bypassSecurityTrustUrl(objectURL);
        }

        console.log(this.currentUser.imageData);
        this.changeEmailForm.controls['email'].setValue(this.currentUser.emailAddress);
        },
      error: (error) => {console.log("No user with this id.", error)}
    })
  }

  ngOnChanges(){
    this.renderMyChart();
  }

  changeEmail(){
    this.webApi.editUserEmail(new UserInfoEditRequest(
      this.currentUserId,
      this.changeEmailForm.value.email!,
      this.changeEmailForm.value.password!)
    ).subscribe({
      next: (res) => {this.currentUser!.emailAddress = res.email},
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
    })
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


  addImage(){
    console.log(this.formData);
    this.webApi.addImageToUser(this.formData, this.currentUserId).subscribe({
      next: (res) => {
        
        let objectURL = 'data:image/png;base64,' + res.imageData;
        this.currentUser.imageData = this.sanitizer.bypassSecurityTrustUrl(objectURL);
        },
        error:(err) => {console.log(err), this.openSnackBar("Something went wrong. Try again!")}
    });
    }
  


  changePassword(){

  }

  deleteUser(){
    this.webApi.deleteUser(this.currentUserId).subscribe({
      next: (res) => {this.logout()},
      error: (error) => {
        this.openSnackBar('Unsuccessful deletion. Try again!')
        console.error('Delete NOT successful!', error);}
    })
  }

  logout(){
    localStorage.removeItem('authToken');
    this.router.navigate([`/login`]);
  }
 
  getReplyCount(){
    this.webApi.getRepliesCount(this.currentUserId).subscribe({
      next: (res) => { this.repliesCount = res, this.renderMyChart();},
      error: (err) => { console.error(err)}
    })
  }

  getPostCount(){
    this.webApi.getPostsCount(this.currentUserId).subscribe({
      next: (res) => { this.postsCount = res, this.renderMyChart();},
      error: (err) => { console.error(err)}
    })
  }

  getPlantCount(){
    this.webApi.getPlantsCount(this.currentUserId).subscribe({
      next: (res) => { this.plantCount = res, this.renderMyChart();},
      error: (err) => { console.error(err)}
    })
  }

  renderMyChart(){
    let chartStatus = Chart.getChart('profileInfo');  

    if (chartStatus != undefined ) {
      chartStatus.destroy();
    };

    const labels = [
      'Posts',
      'Plants',
      'Replies',
      'Items For Sale'
    ];

    const data = {
      labels: labels,
      datasets: [
        {
          data: [this.postsCount, this.plantCount, this.repliesCount, 1],
          backgroundColor: [
            'rgb(255, 99, 132)',
            'rgb(54, 162, 235)',
            'rgb(255, 205, 86)',
            'rgb(255, 23, 13)'
          ],
          hoverOffset: 4
      }]
    };

    let config = new Chart('profileInfo', {
      type: 'doughnut',
      data: data,
      options: {
        responsive: false,
        plugins: {
          legend: {
            position: 'bottom',
          },
          title: {
            display: true,
            text: 'Profile info'
          }
      }
    },
  
    })
    config.update();
  }


  updateChart(chart: Chart ){
    chart.data.datasets[0].data[0] = this.postsCount;
    chart.data.datasets[0].data[1] = this.plantCount;
    chart.data.datasets[0].data[2] = this.repliesCount;
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }
}
