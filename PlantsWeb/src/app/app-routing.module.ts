import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { PostsListComponent } from './posts/posts-list/posts-list.component';
import { PlantsAddComponent } from './plants/plants-add/plants-add.component';
import { PlantsEditComponent } from './plants/plants-edit/plants-edit.component';
import { PlantsListComponent } from './plants/plants-list/plants-list.component';
import { ProfileComponent } from './profile/profile.component';
import { ReplyAddComponent } from './reply-add/reply-add.component';
import { PostAddComponent } from './posts/post-add/post-add.component';

const routes: Routes = [
  {path: '', component: HeaderComponent,
   children: [
    {path: 'main', component: PostsListComponent},
    {path: 'plants', component: PlantsListComponent},
    {path: 'plants/:plantId', component: PlantsEditComponent},
    {path: 'plant/new', component: PlantsAddComponent},
    {path: 'profile', component: ProfileComponent},
   // {path: 'post/:postId', component: ReplyAddComponent},
    {path: 'post/new', component: PostAddComponent}
    //rerouting --> { path: '', redirectTo: 'component-one', pathMatch: 'full' },
  ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
