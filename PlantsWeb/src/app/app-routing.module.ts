import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { MainPageComponent } from './main-page/main-page.component';
import { PlantsAddComponent } from './plants/plants-add/plants-add.component';
import { PlantsEditComponent } from './plants/plants-edit/plants-edit.component';
import { PlantsListComponent } from './plants/plants-list/plants-list.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {path: '', component: HeaderComponent,
   children: [
    {path: 'main', component: MainPageComponent},
    {path: 'plants', component: PlantsListComponent},
    {path: 'plantedit', component: PlantsEditComponent},
    {path: 'plantadd', component: PlantsAddComponent},
    {path: 'profile', component: ProfileComponent}
  ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
