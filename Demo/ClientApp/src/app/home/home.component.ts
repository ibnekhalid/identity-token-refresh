import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/core/services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

 tokenExpiresIn='';
  constructor(public accountService:AccountService) {
   
  }
  ngOnInit() {
    setInterval(()=>{
      this.getExpiryTime();
    },1000)
  }
getUserDetails(){
  if(this.accountService.user)
    return JSON.stringify(this.accountService.user)
    .split(',').join(',<br/>');
  return '';
    
}
getExpiryTime(){
  this.tokenExpiresIn = this.msToHMS(this.accountService.getExpiryTime());
}
 msToHMS( ms:number ) {
  
  var seconds = ms / 1000;
  var hours = seconds / 3600; 
  seconds = seconds % 3600;   
  var minutes = (seconds / 60);  
  seconds = seconds % 60;
  
  return `${hours.toFixed(0)} h ${minutes.toFixed(0)} m ${seconds.toFixed(0)} s`;
}
}
