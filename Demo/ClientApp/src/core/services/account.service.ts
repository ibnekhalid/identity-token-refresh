import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { User, RegisterModel } from '../model/user';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseService {
  private tokenSubject: BehaviorSubject<string>;
  public token: Observable<string>;

private _user : any;
public get user() : any {
  return this._user;
}
public set user(v : any) {
  this._user = v;
}

  constructor(
      private router: Router,
      private http: HttpClient
  ) {
    super();
      this.tokenSubject = new BehaviorSubject<string>(JSON.parse(localStorage.getItem('token')));
      this.token = this.tokenSubject.asObservable();
      this.decodeToken();
  }
  decodeToken() {
    try{
      this.user =  jwt_decode(this.tokenSubject.value);
    }
    catch(Error){
      this.user =null;
    }
  }
  public get tokenValue(): string {
      return this.tokenSubject.value;
  }

  login(username, password) {
      return this.http.post<string>(`${this.baseUrl}/account/login`, { username, password })
          .pipe(map(token => {
              // save token in local storage
              this.setToken(token);
              return token;
          }));
  }

  logout() {
    this.router.navigate(['/auth/login']);
   // remove saved token in local storage
    this.deleteToken();
    
  }

  register(user: RegisterModel) {
      return this.http.post<string>(`${this.baseUrl}/account/register`, user)
      .pipe(map(token => {
        // save token in local storage
        this.setToken(token);
        return token;
    }));
  }
  refreshToken() {
    return this.http.get<string>(`${this.baseUrl}/account/refresh-token`)
    .pipe(map(token => {
      // save token in local storage
      localStorage.setItem('token', JSON.stringify(token));
      this.tokenSubject.next(token);
      this.decodeToken();
      return token;
  }));
}
  getExpiryTime(){
    if(this.user)
    { 
      var d = new Date(0);
     d.setUTCSeconds(this.user.exp);
     var today = new Date()
    var diff =  d.getTime()- today.getTime();
    return diff;
  }
  return 0;
}
setToken(token){
  localStorage.setItem('token', JSON.stringify(token));
  this.tokenSubject.next(token);
  this.decodeToken();
}
deleteToken(){
  localStorage.removeItem('token');
  this.tokenSubject.next(null);
  this.decodeToken();
}
async triggerTokenRefresh(){
  var ms = this.getExpiryTime();
  if(ms>0 && ms<60000){
    await this.refreshToken().toPromise();
  }
}
}
