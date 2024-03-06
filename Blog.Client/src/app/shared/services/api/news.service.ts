import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICreateUserModel, ILoginUserModel, ILoginUserResultModel } from '../../../constants/models/user/user';
import { Observable } from 'rxjs';
import { IApiResult } from '../../../constants/models/api-result/api-result';
import { NEWs_API_ENDPOINTS } from '../../../constants/http/news-api';
import { IListNewsResponseModel } from '../../../constants/models/news/news';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  url: string = '';
  constructor(private readonly _http: HttpClient) 
  { 
  }
  
  listNews(): Observable<IApiResult<IListNewsResponseModel[]>>{    
    return this._http.get<IApiResult<IListNewsResponseModel[]>>(localStorage.getItem('apiUrl') + NEWs_API_ENDPOINTS.List);
  }
}
