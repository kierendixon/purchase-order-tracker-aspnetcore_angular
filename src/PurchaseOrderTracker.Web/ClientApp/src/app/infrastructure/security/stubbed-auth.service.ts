import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class StubbedAuthService implements AuthService {
  private basicUserRole = 'basic_role';
  private superUserRole = 'super_role';
  private authFailedErrorMsg = 'Authentication Failed!';

  public user: string | undefined = undefined;
  public token: string | undefined = undefined;
  private roles: string[] = [];

  public authenticate(username: string, password: string): Observable<string> {
    const that = this;
    return Observable.create(observable => {
      if (username === 'basic' && password === 'basic') {
        that.user = username;
        that.token = that.generateToken(username);
        that.addUserRole(this.basicUserRole);
        observable.next(that.token);
      } else if (username === 'super' && password === 'super') {
        that.user = username;
        that.token = that.generateToken(username);
        that.addUserRole(this.superUserRole);
        observable.next(that.token);
      } else {
        observable.error(this.authFailedErrorMsg);
      }
      observable.complete();
    });
  }

  public isUserAuthenticated(): Observable<boolean> {
    return Observable.create(observable => {
      if (this.token !== undefined && !this.isTokenExpired()) {
        observable.next(true);
      } else {
        observable.next(false);
      }
      observable.complete();
    });
  }

  private generateToken(prefix: string): string {
    return prefix + '_' + Math.random() * 1000;
  }

  private addUserRole(userRole: string): void {
    this.roles.push(userRole);
  }

  private isTokenExpired(): boolean {
    return false;
  }
}
