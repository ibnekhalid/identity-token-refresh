import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { AccountService } from "../services/account.service";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private accountService: AccountService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const token = this.accountService.tokenValue;
        if (token && this.accountService.getExpiryTime()>0) {
            return true;
        }
        this.accountService.deleteToken();
        // not logged in so redirect to login page with the return url
        this.router.navigate(['/auth'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}