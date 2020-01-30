import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';  
import { Login } from 'src/app/models/login';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  
/*
  getHttpHeaders() {
    var a = JSON.parse(localStorage.getItem('userInfo'));
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Bearer': a.token
      })
    };
  }
  */

  url = 'https://localhost:44303/api/';

  getHttpHeaders() {
    var a = JSON.parse(localStorage.getItem('userInfo'));
    return {
      headers: new HttpHeaders({
        //'Content-Type': 'application/json',
        'Authorization': "Bearer "+a.token
      })
    };
  }

  constructor(private http: HttpClient) { }


  postCreateAccount(login: Login): any{
    return this.http.post(this.url+"usuarios/criar", login);
  }

  getInformationUser(email: string): any{
    return this.http.get<User>(this.url+"usuarios/users?"+"email="+email);
  }

  postLogin(login: Login): any{
      return this.http.post(this.url+"usuarios/login", login);
  }  

}
