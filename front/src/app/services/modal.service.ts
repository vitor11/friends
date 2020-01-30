import { Injectable } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../modal/modal.component';
import { CalculoLog } from '../models/calculo-log';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  role:[];

  constructor(private modalService: BsModalService) { }

  showConfirmMessage(title: string, response: [], okTxt?:string, cancelTxt?: string){
    const bsModalRef: BsModalRef = this.modalService.show(ModalComponent);
    bsModalRef.setClass('modal-xl');
    bsModalRef.content.title = title;
    bsModalRef.content.message = response;

    if(okTxt){
      bsModalRef.content.okTxt = okTxt;
    }
    if(cancelTxt){
      bsModalRef.content.cancelTxt = cancelTxt;
    }

    return (<ModalComponent>bsModalRef.content).confirmResult;
  }

  
}
