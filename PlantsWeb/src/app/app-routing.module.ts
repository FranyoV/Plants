import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { PostsListComponent } from './posts/posts-list/posts-list.component';
import { PlantsAddComponent } from './plants/plants-add/plants-add.component';
import { PlantsEditComponent } from './plants/plants-edit/plants-edit.component';
import { PlantsListComponent } from './plants/plants-list/plants-list.component';
import { ProfileComponent } from './profile/profile.component';
import { PostDetailsComponent } from './posts/post-details/post-details.component';
import { PostAddComponent } from './posts/post-add/post-add.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MarketplaceComponent } from './marketplace/item-list/marketplace.component';
import { ItemAddComponent } from './marketplace/item-add/item-add.component';
import { ItemEditComponent } from './marketplace/item-edit/item-edit.component';
import { AuthGuardService } from './auth-guard.service';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},  
  {path: '', component: HeaderComponent, canActivate: [AuthGuardService],
   children: [
    {path: 'main', component: PostsListComponent},
    {path: 'plants', component: PlantsListComponent},
    {path: 'plants/:plantId', component: PlantsEditComponent},
    {path: 'plant/new', component: PlantsAddComponent},
    {path: 'profile', component: ProfileComponent},
    {path: 'post/:postId', component: PostDetailsComponent},
    {path: 'posts/new', component: PostAddComponent},
    {path: 'marketplace', component: MarketplaceComponent},
    {path: 'item/new', component: ItemAddComponent},
    {path: 'items/:itemId', component: ItemEditComponent}
    ]
    //rerouting --> { path: '', redirectTo: 'component-one', pathMatch: 'full' },
  },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
