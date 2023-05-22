import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Plant } from './models/Plant';
import { Post } from './models/Post';
import { Item } from './models/Item';
import { ItemDto } from './models/ItemDto';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  plants : Plant[] = [];
  posts : Post[] = [];
  items : ItemDto[] = [];
  userId : string = '';
  currentUserId : string = "41b37237-054b-4bd5-79d1-08db48b0dc15";

  //holds current value of the message
  private plantsMessageSource = new BehaviorSubject<Plant[]>(this.plants);
  private postsMessageSource = new BehaviorSubject<Post[]>(this.posts);
  private itemsMessageSource = new BehaviorSubject<ItemDto[]>(this.items);
  private userIdMessageSource = new BehaviorSubject<string>(this.userId);
  private currentUserIdMessageSource = new BehaviorSubject<string>(this.currentUserId);

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

  changeItemsMessage(data: ItemDto[]){
    this.itemsMessageSource.next(data);
  }

  changeUserIdMessage(data: string){
    this.userIdMessageSource.next(data);
  }


}
