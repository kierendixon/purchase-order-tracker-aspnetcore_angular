

export class UserService {
    public async handle(): Promise<void> {
        var result = await fetch('/user');
        console.log("result: " + JSON.stringify(result))
        
    }
}





// import { HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Observable } from 'rxjs';

// import { baseUserUrl } from '../../config/api.config';

// @Injectable()
// export class UserService {
//   constructor(private http: HttpClient) {}

//   public handle(): Observable<User> {
//     return this.http.get<User>(baseUserUrl);
//   }
// }

// export interface User {
//   username: string;
//   isAdmin: boolean;
// }
