import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/core/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent  implements OnInit {
  
  title = 'app';
  constructor(private accountService:AccountService){
  }
  ngOnInit() {
    this.accountService.token.subscribe(x => {
      if( x && x.length>0)
         setInterval(async ()=>{await this.accountService.triggerTokenRefresh()},5000);
  });
    
  }
}
