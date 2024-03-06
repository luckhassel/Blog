import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Blog.Client';

  constructor(private _http: HttpClient){
    this.getBaseUrl().subscribe((data) => {
      localStorage.setItem('apiUrl', data.apiServer.url);
      console.log(`[AppComponent][Url]${JSON.stringify(data)}`);
    });
  }

  private getBaseUrl(): Observable<any>{
    return this._http.get('./assets/config/config.json')
  }
}
