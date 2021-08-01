import { Component } from '@angular/core';
import { AccountService } from 'src/core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isLoggedIn=false;

  constructor(private accountService: AccountService,
    private router: Router) {
      this.accountService.token.subscribe(x => {this.isLoggedIn = x && x.length>0});
  }
  username(){
    if(this.accountService.user)
      return this.accountService.user.unique_name;
    return '';
      
  }
  logout() {
      this.accountService.logout();
  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
