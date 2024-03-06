import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICreateUserModel, ILoginUserModel, ILoginUserResultModel } from '../../../constants/models/user/user';
import { Observable } from 'rxjs';
import { USER_API_ENDPOINTS } from '../../../constants/http/user-api';
import { IApiResult } from '../../../constants/models/api-result/api-result';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = '';
  constructor(private readonly _http: HttpClient) 
  { 
  }

  createUser(user: ICreateUserModel): Observable<any>{    
    return this._http.post<any>(localStorage.getItem('apiUrl') + USER_API_ENDPOINTS.Create, 
      user
    );
  }

  loginUser(user: ILoginUserModel): Observable<IApiResult<ILoginUserResultModel>>{    
    return this._http.post<IApiResult<ILoginUserResultModel>>(localStorage.getItem('apiUrl') + USER_API_ENDPOINTS.Login, 
      user
    );
  }
}
