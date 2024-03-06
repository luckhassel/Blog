import { Component } from '@angular/core';
import {MatExpansionModule} from '@angular/material/expansion';
import { NewsService } from '../../shared/services/api/news.service';
import { IListNewsResponseModel } from '../../constants/models/news/news';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-news-list',
  standalone: true,
  imports: [MatExpansionModule, 
    FlexLayoutModule,
    CommonModule
  ],
  templateUrl: './news-list.component.html',
  styleUrl: './news-list.component.css'
})
export class NewsListComponent {
  panelOpenState = false;
  news: IListNewsResponseModel[] = [];

  constructor(private _newsService: NewsService){
    this._newsService.listNews().subscribe((data) => {
      console.log(`[NewsListComponent] Success -> ${JSON.stringify(data)}`);
      this.news = data.value;
    },
    (error) => {
      console.log(`[NewsListComponent] Error -> ${JSON.stringify(error)}`);
    });
  }


}
