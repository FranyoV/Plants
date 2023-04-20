import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Plant } from './models/Plant';
import { Post } from './models/Post';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  plants : Plant[] = [];
  posts : Post[] = [];

  //holds current value of the message
  private plantsMessageSource = new BehaviorSubject<Plant[]>(this.plants);
  private postsMessageSource = new BehaviorSubject<Post[]>(this.posts);
  private plantId = new BehaviorSubject<string>("");

  currentPlantsMessage = this.plantsMessageSource.asObservable();
  currentPostsMessage = this.postsMessageSource.asObservable();
  currentPlantIdMessage = this.plantId.asObservable();

  constructor() { }

  changePlantsMessage(data: Plant[]){
    this.plantsMessageSource.next(data);
  }

  changePostsMessage(data: Post[]){
    this.postsMessageSource.next(data);
  }

  sendPlantId(id: string){
    this.plantId.next(id);
  }

}
