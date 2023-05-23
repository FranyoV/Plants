import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  currentUserId : string = '';
  constructor() { }

  LoggedInUser() : string{
    var myRawToken = localStorage.getItem("authToken");
    const tokenInfo = this.getDecodedAccessToken(myRawToken!); // decode token
    //const expireDate = tokenInfo.exp; // get token expiration dateTime
    console.log(tokenInfo["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]); // show decoded token object in console
    this.currentUserId = tokenInfo["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]; 
    return this.currentUserId;
  }


  getDecodedAccessToken(token: string): any {
    try {
      return jwt_decode(token);
    } catch(Error) {
      return null;
    }
  }
}


