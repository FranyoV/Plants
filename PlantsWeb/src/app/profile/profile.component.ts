import { Component, OnInit } from '@angular/core';
import { User } from '../models/User';
import { WebApiService } from '../webapi.service';
import { ActivatedRoute } from '@angular/router';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  currentUserId!: string;
  currentUser!: User | undefined;

  constructor(
    private webApi: WebApiService,
    private route: ActivatedRoute
  ){}

  ngOnInit(): void {

    this.route.paramMap.subscribe( (params) => {
      const id = params.get("userId");
      this.currentUserId = id!;
    } )

    this.webApi.getUserById(this.currentUserId).subscribe({
      next: (res) => { this.currentUser = res},
      error: (error) => {console.log("No user with this id.", error)}
    })

    this.renderMyChart();
  }

  ngOnChanges(){
    this.renderMyChart();
  }

  renderMyChart(){
    let chartStatus = Chart.getChart('myChart');  
console.log(chartStatus);

    const DATA_COUNT = 3;
    const NUMBER_CFG = {count: DATA_COUNT, min: -100, max: 100};

    const labels = [
      'Posts',
      'Plants',
      'Replies'
    ];
    const data = {
      labels: labels,
      datasets: [
        {
          label: 'My First Dataset',
          data: [300, 50, 100],
          backgroundColor: [
            'rgb(255, 99, 132)',
            'rgb(54, 162, 235)',
            'rgb(255, 205, 86)'
          ],
          hoverOffset: 4
      }]
    };

    const config = new Chart('myChart', {
      type: 'doughnut',
      data: data,
      options: {
        responsive: false,
        plugins: {
          legend: {
            position: 'left',
          },
          title: {
            display: true,
            text: 'Profile info'
          }
      }
    },
  
    })
    console.log(chartStatus);

  }

}
