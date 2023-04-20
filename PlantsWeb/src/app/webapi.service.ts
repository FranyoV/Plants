import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Plant } from './models/Plant';
import { Post } from './models/Post';
import { Reply } from './models/Reply';
import { Guid } from 'guid-typescript';
import { PlantDto } from './models/PlantDto';
import { User } from './models/User';

@Injectable({
  providedIn: 'root'
})

export class WebApiService {

  //todo: check localhost port on backend appsettings/launchsettings
  baseUrl: string = "https://localhost:7050";
  httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'})};

  constructor(public http: HttpClient) { }

  //Auth
/*register(request: LogingRequest): Observable<any> {
    let url = `${this.baseUrl}/api/auth/register`
    return this.http.post<any>(url, request);
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    let url = `${this.baseUrl}/api/auth/login`;
    return this.http.post<LoginResponse>(url, request);
  }

  getMe(): Observable<string> {
    let url = `${this.baseUrl}/api/auth`;
    return this.http.get(url, {responseType: 'text'});
  }*/


  getUserById(id: string): Observable<User> {
    let url = `${this.baseUrl}/api/users/${id}`;
    return this.http.get<User>(url);
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

  //POSTS
  getPosts(): Observable<Post[]>{
    const url = `${this.baseUrl}/api/posts`;
    return this.http.get<Post[]>(url);
  }

  getPostById(id: string): Observable<Post> {
    let url = `${this.baseUrl}/api/posts/${id}`;
    return this.http.get<Post>(url);
  }

  addPost(post: Post): Observable<Post> {
    let url = `${this.baseUrl}/api/posts`;
    return this.http.post<Post>(url, post, this.httpOptions);
  }

  updatePost(id: string, post: Post): Observable<any> {
    let url = `${this.baseUrl}/api/posts/${id}`;
    return this.http.put(url, post, this.httpOptions);
  }

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

  getRepliesOfPost(postId: string) : Observable<Reply[]> {
    let url = `${this.baseUrl}/api/replies/post/${postId}`;
    return this.http.get<Reply[]>(url);
  }

  addReplies(reply: Reply): Observable<Reply> {
    let url = `${this.baseUrl}/api/replies`;
    return this.http.post<Reply>(url, reply, this.httpOptions);
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
