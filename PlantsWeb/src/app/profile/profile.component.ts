import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { User } from '../models/User';
import { WebApiService } from '../webapi.service';
import { ActivatedRoute } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { FormBuilder, Validators } from '@angular/forms';
Chart.register(...registerables);


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnChanges {

  currentUserId!: string;
  currentUser!: User | undefined;
  repliesCount: number = 1;
  postsCount: number = 1;
  plantCount: number = 1;
  chart!: Chart;

  
  editForm = this.formBuilder.group({
    username : ['', [Validators.required, Validators.maxLength(8)]],
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

  constructor(
    private webApi: WebApiService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder
  ){}

  ngOnInit(): void {

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("userId");
      this.currentUserId = id!;
    } )

    this.webApi.getUserById(this.currentUserId).subscribe({
      next: (res) => { this.currentUser = res;
        this.editForm.controls['username'].setValue(res.name);
        this.editForm.controls['email'].setValue(res.email);
        },
      error: (error) => {console.log("No user with this id.", error)}
    })
    this.getPlantCount();
    this.getReplyCount();
    this.getPostCount();

    this.renderMyChart();
    
  }

  ngOnChanges(){
    this.renderMyChart();
    console.log("nyeh")
  }

  changePassword(){

  }

  changeProfileData(){

  }
 
  getReplyCount(){
    this.webApi.getRepliesCount(this.currentUserId).subscribe({
      next: (res) => { this.repliesCount = res, console.log(res)},
      error: (err) => { console.error(err)}
    })
  }

  getPostCount(){
    this.webApi.getPostsCount(this.currentUserId).subscribe({
      next: (res) => { this.postsCount = res, console.log(res)},
      error: (err) => { console.error(err)}
    })
  }

  getPlantCount(){
    this.webApi.getPlantsCount(this.currentUserId).subscribe({
      next: (res) => { this.plantCount = res, console.log(res)},
      error: (err) => { console.error(err)}
    })
  }



  renderMyChart(){
    let chartStatus = Chart.getChart('profileInfo');  
    console.log(chartStatus);

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
          //label: 'My First Dataset',
          data: [this.postsCount, this.plantCount, this.repliesCount, 2],
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
            position: 'top',
          },
          title: {
            display: true,
            text: 'Profile info'
          }
      }
    },
  
    })
    console.log(chartStatus);
    config.update();
  }


  updateChart(chart: Chart ){
    chart.data.datasets[0].data[0] = this.postsCount;
    chart.data.datasets[0].data[1] = this.plantCount;
    chart.data.datasets[0].data[2] = this.repliesCount;

  }
}