import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Plant } from './models/Plant';
import { Post } from './models/Post';
import { Reply } from './models/Reply';
import { Guid } from 'guid-typescript';
import { PlantDto } from './models/PlantDto';
import { User } from './models/User';
import { LoginRequest } from './models/LoginRequest';
import { LoginResponse } from './models/LoginResponse';
import { Item } from './models/Item';
import { RegisterRequest } from './models/RegisterRequest';
import { UserInfoEditRequest } from './models/UserInfoEditRequest';
import { AbstractControl } from '@angular/forms';
import { ReplyDto } from './models/ReplyDto';
import { ItemDto } from './models/ItemDto';

@Injectable({
  providedIn: 'root'
})

export class WebApiService {

  //todo: check localhost port on backend appsettings/launchsettings
  baseUrl: string = "https://localhost:7050";
  httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'})};

  constructor(public http: HttpClient) { }

  //Auth
  register(request: RegisterRequest): Observable<any> {
    let url = `${this.baseUrl}/api/account/register`
    return this.http.post<any>(url, request);
  }

  validateUserName(username : AbstractControl) : Observable<any>{
    let url = `${this.baseUrl}/api/account/${username.value}`;
    return this.http.get<any>(url)
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    let url = `${this.baseUrl}/api/account/login`;
    return this.http.post<LoginResponse>(url, request);
  }

  getMe(): Observable<string> {
    let url = `${this.baseUrl}/api/account`;
    return this.http.get(url, {responseType: 'text'});
  }

  getUserById(id: string): Observable<User> {
    let url = `${this.baseUrl}/api/account/${id}`;
    return this.http.get<User>(url);
  }

  editUserEmail( request:UserInfoEditRequest) : Observable<any>{
    let url = `${this.baseUrl}/api/account/${request.userId}`;
    return this.http.put(url, request, this.httpOptions);
  }

  deleteUser(userId: string): Observable<User> {
    let url = `${this.baseUrl}/api/account/${userId}`;
    return this.http.delete<User> (url, this.httpOptions);
  }


  //PLANTS
  getPlants(): Observable<Plant[]>{
    const url = `${this.baseUrl}/api/plants`;
    return this.http.get<Plant[]>(url);
  }

  getPlantById(id: string): Observable<Plant> {
    let url = `${this.baseUrl}/api/plants/${id}`;
    return this.http.get<Plant>(url);
  }

  getPlantsOfUser(userId: string): Observable<Plant[]> {
    let url = `${this.baseUrl}/api/plants/user/${userId}`;
    return this.http.get<Plant[]>(url);
  }

  getPlantsCount(userId: string) : Observable<number>{
    let url = `${this.baseUrl}/api/plants/user/${userId}/count`;
    return this.http.get<number>(url);
  }

  addPlant(plant: Plant): Observable<Plant> {
    let url = `${this.baseUrl}/api/plants`;
    return this.http.post<Plant>(url, plant);
  }

  editPlant(id: string, plant: Plant): Observable<any> {
    let url = `${this.baseUrl}/api/plants/${id}`;
    return this.http.put(url, plant, this.httpOptions);
  }

  deletePlant(id: string): Observable<Plant> {
    let url = `${this.baseUrl}/api/plants/${id}`;
    return this.http.delete<Plant>(url, this.httpOptions);
  }


  //ITEMS
  getItems(): Observable<ItemDto[]>{
    const url = `${this.baseUrl}/api/items`;
    return this.http.get<ItemDto[]>(url);
  }

  getItemsById(id: string): Observable<Item> {
    let url = `${this.baseUrl}/api/items/${id}`;
    return this.http.get<Item>(url);
  }

  getItemsOfUser(userId: string): Observable<Item[]> {
    let url = `${this.baseUrl}/api/items/user/${userId}`;
    return this.http.get<Item[]>(url);
  }

  getItemsCount(userId: string) : Observable<number>{
    let url = `${this.baseUrl}/api/items/user/${userId}/count`;
    return this.http.get<number>(url);
  }

  addItem(item: Item): Observable<Item> {
    let url = `${this.baseUrl}/api/items`;
    return this.http.post<Item>(url, item);
  }

  editItem(id: string, item: Item): Observable<any> {
    let url = `${this.baseUrl}/api/items/${id}`;
    return this.http.put(url, item, this.httpOptions);
  }

  deleteItem(id: string): Observable<Item> {
    let url = `${this.baseUrl}/api/items/${id}`;
    return this.http.delete<Item>(url, this.httpOptions);
  }

  //POSTS
  getPosts(): Observable<Post[]>{
    const url = `${this.baseUrl}/api/posts`;
    return this.http.get<Post[]>(url);
  }

  getPostById(id: string): Observable<Post> {
    let url = `${this.baseUrl}/api/posts/${id}`;
    return this.http.get<Post>(url);
  }

  getPostByUser(userId: string): Observable<Post[]> {
    let url = `${this.baseUrl}/api/posts/${userId}/posts`;
    return this.http.get<Post[]>(url);
  }

  getPostByUserReplies(userId: string): Observable<Post[]> {
    let url = `${this.baseUrl}/api/posts/${userId}/replies`;
    return this.http.get<Post[]>(url);
  }

  getPostsCount(userId: string) : Observable<number>{
    let url = `${this.baseUrl}/api/posts/user/${userId}/count`;
    return this.http.get<number>(url);
  }

  addPost(post: Post): Observable<Post> {
    let url = `${this.baseUrl}/api/posts`;
    return this.http.post<Post>(url, post, this.httpOptions);
  }

 /* updatePost(id: string, post: Post): Observable<any> {
    let url = `${this.baseUrl}/api/posts/${id}`;
    return this.http.put(url, post, this.httpOptions);
  }*/

  deletePost(id: string): Observable<Post> {
    let url = `${this.baseUrl}/api/posts/${id}`;
    return this.http.delete<Post>(url, this.httpOptions);
  }

  //REPLIES
  getReplies(): Observable<Reply[]>{
    const url = `${this.baseUrl}/api/replies`;
    return this.http.get<Reply[]>(url);
  }

  getRepliesById(id: Guid): Observable<Reply> {
    let url = `${this.baseUrl}/api/replies/${id}`;
    return this.http.get<Reply>(url);
  }

  getRepliesOfPost(postId: string) : Observable<ReplyDto[]> {
    let url = `${this.baseUrl}/api/replies/post/${postId}`;
    return this.http.get<ReplyDto[]>(url);
  }

  getRepliesCount(userId: string) : Observable<number>{
    let url = `${this.baseUrl}/api/replies/user/${userId}/count`;
    return this.http.get<number>(url);
  }

  addReplies(reply: Reply): Observable<ReplyDto> {
    let url = `${this.baseUrl}/api/replies`;
    return this.http.post<ReplyDto>(url, reply, this.httpOptions);
  }

  updateReply(id: Guid, reply: Reply): Observable<any> {
    let url = `${this.baseUrl}/api/replies/${id}`;
    return this.http.put(url, reply, this.httpOptions);
  }

  deleteReply(id: Guid): Observable<Reply> {
    let url = `${this.baseUrl}/api/replies/${id}`;
    return this.http.delete<Reply>(url, this.httpOptions);
  }


}
