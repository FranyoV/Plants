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

  private plantsDataSource = new BehaviorSubject<Plant[]>(this.plants);
  private postsDataSource = new BehaviorSubject<Post[]>(this.posts);
  private plantId = new BehaviorSubject<string>("");

  currentPlants = this.plantsDataSource.asObservable();
  currentPosts = this.postsDataSource.asObservable();
  currentPlantId = this.plantId.asObservable();

  constructor() { }

  updatePlantsList(data: Plant[]){
    this.plantsDataSource.next(data);
    console.log("message in dataservice");
  }

  updatePostsList(data: Post[]){
    this.postsDataSource.next(data);
    console.log("in data service for postslist update");
  }

  sendPlantId(id: string){
    this.plantId.next(id);
    console.log("plant id: ", id)
  }
}
