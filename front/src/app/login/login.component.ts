import { Component, OnInit } from '@angular/core';
import { LoginService } from '../services/login.service';
import {FormGroup, FormBuilder} from '@angular/forms';
import { Login } from '../models/login';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  formLogin: FormGroup;
  login: Login;
  result: any;

  constructor( private formBuilder: FormBuilder,
    private loginService: LoginService,
    private router: Router) {
    }

    ngOnInit() {}

  createForm(email, password){
    this.formLogin = this.formBuilder.group({
      password: password,
      email: email
    });
  }

  loginUser(email, password){
    this.createForm(email, password);
  
    this.loginService.postLogin(this.formLogin.value).subscribe(response => {
      if(typeof(response.erro) != "undefined"){
          alert("ERRO DE LOGIN");
      }
      else{
        this.loginService.getInformationUser(email).subscribe(response => {
          this.result = response;
          localStorage.setItem('userInfo', JSON.stringify(this.result));
          this.router.navigate(['/calculo']);
        })
      }
    }, error => {
        alert("Insira credenciais válidas");
    });
  }


  createAccount(email, password){
    this.createForm(email, password);
    this.loginService.postCreateAccount(this.formLogin.value).subscribe(response => {
      //localStorage.setItem('userInfo', JSON.stringify(response));
        if(typeof(response.erro) != "undefined"){
          alert("Erro ao cadastrar");
        }
        else{
          this.loginService.getInformationUser(email).subscribe(response => {
            this.result = response;
            localStorage.setItem('userInfo', JSON.stringify(response));
            alert("Usuário adicionado");
            this.router.navigate(['/calculo']);
          }, error => {
              console.log("ERRO1 "+error.erro);
              //alert("Erro ao cadastrar");
        });
        }
        //this.router.navigate(['/calculo']);
    }, error => {
      console.log("ERRO1 "+error.erro);
        alert("Erro ao cadastrar");
  });
  }

}
