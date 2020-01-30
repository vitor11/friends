import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CalculoLog } from '../models/calculo-log';

@Injectable({
  providedIn: 'root'
})
export class CalculoLogService {

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

  
  getCloseFriends(userId: number, LatitudeA: number, LongitudeA: number): any{
    return this.http.get<CalculoLog[]>(this.url+"usuarios/closefriends?"+"userId="+userId+"&LatitudeA="+LatitudeA+"&LongitudeA="+LongitudeA, this.getHttpHeaders());
  }
  
  /*getFriend(id: any): Observable<CalculoLog> {  
    return this.http.get<CalculoLog>(this.url+"usuarios/"+id);  
  }

  getAllFriends(): Observable<CalculoLog[]> {  
    return this.http.get<CalculoLog[]>(this.url+"usuarios", this.getHttpHeaders());  
  }*/
  
}
