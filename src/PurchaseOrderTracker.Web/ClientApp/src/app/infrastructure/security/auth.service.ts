import { Observable } from 'rxjs';

export interface AuthService {
  authenticate(username: string, password: string): Observable<string>;
  isUserAuthenticated(): Observable<boolean>;
}
