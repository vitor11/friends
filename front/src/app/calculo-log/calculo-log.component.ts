import { Component, OnInit } from '@angular/core';
import { CalculoLog } from '../models/calculo-log';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import { CalculoLogService } from '../services/calculo-log.service';
import { FriendService } from '../services/friend.service';
import { Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal/public_api';
import { ModalService } from '../services/modal.service';

@Component({
  selector: 'app-calculo-log',
  templateUrl: './calculo-log.component.html',
  styleUrls: ['./calculo-log.component.scss']
})
export class CalculoLogComponent implements OnInit {

  formCalculo: FormGroup;
  formFriend: FormGroup;
  calculo: CalculoLog;
  result: any;
  roles: [];
  friend: [];
  local: string;

  constructor( private formBuilder: FormBuilder,
    private calculoService: CalculoLogService,
    private friendService: FriendService,
    private router: Router,
    private modalService: ModalService) {
      this.local = JSON.stringify(localStorage);
    }

    ngOnInit() {

      //getFriends
      var a = JSON.parse(localStorage.getItem('userInfo'));
      if(a==null){
        this.router.navigate(['/']);
      }
      if(typeof(a.userId) == 'undefined'){
        this.router.navigate(['/calculo']);
      }
      else{
        this.friendService.getFriends(a.userId).subscribe(response => {
          this.roles = response;
        }, error => {
          if(error.status == 401)
            alert("Login expirado");
          else
            alert("Erro ao acessar ");
        });
      }
  }

  openModal(resp){
    const result$ = this.modalService.showConfirmMessage('Amigos mais próximos', resp);
  }

  getAllFriends(){
    var b = JSON.parse(localStorage.getItem('userInfo'));
    this.friendService.getFriends(b.userId).subscribe(response => {
      this.roles = response;
    });
  }


  createFormFriend(UserID, name, LatitudeB, LongitudeB){
    this.formFriend = this.formBuilder.group({
      UserID: UserID,
      name: name,
      LatitudeB: LatitudeB,
	    LongitudeB: LongitudeB
    });
  }

  createFormPutFriend(friendId, UserID, name, LatitudeB, LongitudeB){
    this.formFriend = this.formBuilder.group({
      FriendId: friendId,
      UserID: UserID,
      name: [name ? name : '', Validators.required],
      LatitudeB: [LatitudeB ? LatitudeB : '', Validators.required],
	    LongitudeB: [LongitudeB ? LongitudeB  : '', Validators.required]
    });
  }

  logout(){
    localStorage.removeItem('userInfo');
    this.router.navigate(['/']);
  }

  calculate(LatitudeA, LongitudeA){
    if(LatitudeA == '' || LongitudeA == ''){
      alert("Os campos são obrigatórios para realizar a pesquisa");
    }
    else{
      var calculate = JSON.parse(localStorage.getItem('userInfo'));
      this.calculoService.getCloseFriends(calculate.userId, LatitudeA, LongitudeA).subscribe(response => {
        this.friend = response;
        this.openModal(response);
      }, error => {
        if(error.status == 401)
          alert("Login expirado");
        else
          alert("Erro ao pesquisar, tente novamente");
      });
    }
  }

  update(friendId, name, LatitudeB, LongitudeB ){
    var updt = JSON.parse(localStorage.getItem('userInfo'));
    this.createFormPutFriend(friendId, updt.userId ,name, LatitudeB, LongitudeB);
    this.friendService.putFriend(friendId, this.formFriend.value).subscribe(response => {
      localStorage.removeItem('userInfo');
      localStorage.setItem('userInfo', JSON.stringify(updt));

      this.getAllFriends();
    }, error => {
      if(error.status == 401)
          alert("Login expirado");
      else
        alert("Erro ao alterar "+error.error);
    });
  }

  delete(friendId){
    var del = JSON.parse(localStorage.getItem('userInfo'));
    this.friendService.deleteFriend(friendId).subscribe(response => {

      localStorage.removeItem('userInfo');
      localStorage.setItem('userInfo', JSON.stringify(del));

      this.getAllFriends();

    }, error => {
      if(error.status == 401)
          alert("Login expirado");
      else
        alert("Erro ao excluir");
    });
  }

  save(name, LatitudeB, LongitudeB){
    var save = JSON.parse(localStorage.getItem('userInfo'));
    this.createFormFriend(save.userId ,name, LatitudeB, LongitudeB);
    this.friendService.postAddFriend(this.formFriend.value).subscribe(response => {
        alert("Amigo adicionado");

        localStorage.removeItem('userInfo');
        localStorage.setItem('userInfo', JSON.stringify(save));

        this.createFormFriend
        this.getAllFriends();
    }, error => {
      if(error.status == 401)
          alert("Login expirado");
      else
        alert("Erro ao cadastrar");
    });
  }


}
