import { Routes } from '@angular/router';
import { UserRegistryComponent } from './views/user-registry/user-registry.component';
import { UserLoginComponent } from './views/user-login/user-login.component';
import { UserGuard } from './shared/guards/user.guard';
import { AppGuard } from './shared/guards/app.guard';
import { NewsListComponent } from './views/news-list/news-list.component';

export const routes: Routes = [
    {path: '', component: UserLoginComponent, canActivate: [UserGuard]},
    {path: 'registry', component: UserRegistryComponent, canActivate: [UserGuard]},
    {path: 'news', component: NewsListComponent, canActivate: [AppGuard]}
];
