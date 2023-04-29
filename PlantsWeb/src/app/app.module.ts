import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { AppComponent } from './app.component';
import { PostsListComponent } from './posts/posts-list/posts-list.component';
import { PlantsListComponent } from './plants/plants-list/plants-list.component';
import { HeaderComponent } from './header/header.component';
import { PlantsAddComponent } from './plants/plants-add/plants-add.component';
import { PlantsEditComponent } from './plants/plants-edit/plants-edit.component';
import { ProfileComponent } from './profile/profile.component';
import { DialogComponent } from './plants/plants-list/dialog/dialog.component';
import { PostFilterPipe } from './post-filter.pipe';
import { PostDetailsComponent } from './posts/post-details/post-details.component';
import { PostAddComponent } from './posts/post-add/post-add.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MarketplaceComponent } from './marketplace/item-list/marketplace.component';
import { ItemAddComponent } from './marketplace/item-add/item-add.component';
import { ItemDetailsComponent } from './marketplace/item-details/item-details.component';
import { ItemEditComponent } from './marketplace/item-edit/item-edit.component';
import { SnackbarComponent } from './snackbar/snackbar.component';
import { AuthInterceptor } from './auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    PostsListComponent,
    PlantsListComponent,
    HeaderComponent,
    PlantsAddComponent,
    PlantsEditComponent,
    ProfileComponent,
    DialogComponent,
    PostFilterPipe,
    PostDetailsComponent,
    PostAddComponent,
    LoginComponent,
    RegisterComponent,
    MarketplaceComponent,
    ItemAddComponent,
    ItemDetailsComponent,
    ItemEditComponent,
    SnackbarComponent
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
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatExpansionModule,
    MatListModule,
    MatTooltipModule,
    MatTableModule,
    MatTabsModule,
    MatSelectModule,
    MatRadioModule,
    MatSnackBarModule
  ],
  providers: [{provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
