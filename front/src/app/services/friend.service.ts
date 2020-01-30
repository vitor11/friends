import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Friend } from 'src/app/models/friend';
import { CalculoLog } from '../models/calculo-log';

@Injectable({
  providedIn: 'root'
})
export class FriendService {
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


  postAddFriend(friend: Friend): any{
    return this.http.post(this.url+"usuarios/add", friend, this.getHttpHeaders());
  }

  getFriends(usrId: number): any{
    return this.http.get<Friend[]>(this.url+"usuarios/all?"+"userId="+usrId, this.getHttpHeaders());
  }

  deleteFriend(firndId: number): any{
    return this.http.delete<Friend[]>(this.url+"usuarios"+"/"+firndId, this.getHttpHeaders());
  }

  putFriend(friendId: number, friend: Friend): any{
    return this.http.put<Friend[]>(this.url+"usuarios"+"/"+friendId, friend, this.getHttpHeaders());
  }

}
