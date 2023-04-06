import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatPaginatorModule } from '@angular/material/paginator';

import { AppComponent } from './app.component';
import { MainPageComponent } from './main-page/main-page.component';
import { PlantsListComponent } from './plants/plants-list/plants-list.component';
import { HeaderComponent } from './header/header.component';
import { PlantsAddComponent } from './plants/plants-add/plants-add.component';
import { PlantsEditComponent } from './plants/plants-edit/plants-edit.component';
import { ProfileComponent } from './profile/profile.component';
import { DialogComponent } from './dialog/dialog.component';
import { PostFilterPipe } from './post-filter.pipe';
import { ReplyAddComponent } from './reply-add/reply-add.component';
import { PostAddComponent } from './post-add/post-add.component';


@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    PlantsListComponent,
    HeaderComponent,
    PlantsAddComponent,
    PlantsEditComponent,
    ProfileComponent,
    DialogComponent,
    PostFilterPipe,
    ReplyAddComponent,
    PostAddComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    MatButtonToggleModule,
    MatPaginatorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
