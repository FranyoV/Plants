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
    var tkn = localStorage.getItem("authToken");
    const decodedTkn = this.getDecodedAccessToken(tkn!); // decode token
    this.currentUserId = decodedTkn["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]; 
    return this.currentUserId;
  }

  isExpired() : boolean{
    var tkn = localStorage.getItem("authToken");
    const decodedTkn = this.getDecodedAccessToken(tkn!); // decode token
    const expireDate = decodedTkn.exp; // get token expiration dateTime
    if (expireDate < new Date() ){
      return false;
    }
    else{
      return true;
    }
  }

  getDecodedAccessToken(tkn: string) : any {
    try {
      return jwt_decode(tkn);
    } catch(Error) {
      return null;
    }
  }
}


