import { Observable } from 'rxjs/Observable';

export interface AuthService {
    authenticate(username: string, password: string): Observable<string>;
    isUserAuthenticated(): Observable<boolean>;
}
