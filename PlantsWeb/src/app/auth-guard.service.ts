import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export  class AuthGuardService implements CanActivate{
 
  constructor(private userService : UserService,
    private router : Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var userId = this.userService.LoggedInUser;
    if (userId != null && !this.userService.isExpired()){
        return true;     
    }else{
      this.router.navigate(['login']);
      return false;
    }

  }
}
