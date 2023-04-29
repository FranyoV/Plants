import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Plant } from './models/Plant';
import { Post } from './models/Post';
import { Item } from './models/Item';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  plants : Plant[] = [];
  posts : Post[] = [];
  items : Item[] = [];
  userId : string = '';

  //holds current value of the message
  private plantsMessageSource = new BehaviorSubject<Plant[]>(this.plants);
  private postsMessageSource = new BehaviorSubject<Post[]>(this.posts);
  private itemsMessageSource = new BehaviorSubject<Item[]>(this.items);
  private userIdMessageSource = new BehaviorSubject<string>(this.userId);

  currentPlantsMessage = this.plantsMessageSource.asObservable();
  currentPostsMessage = this.postsMessageSource.asObservable();
  currentItemsMessage = this.itemsMessageSource.asObservable();
  currentUserIdMessage = this.userIdMessageSource.asObservable();

  constructor() { }

  changePlantsMessage(data: Plant[]){
    this.plantsMessageSource.next(data);
  }

  changePostsMessage(data: Post[]){
    this.postsMessageSource.next(data);
  }

  changeItemsMessage(data: Item[]){
    this.itemsMessageSource.next(data);
  }

  changeUserIdMessage(data: string){
    this.userIdMessageSource.next(data);
  }


}
