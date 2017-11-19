import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthenticationService } from './authentication.service';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { RestrictedComponent } from './restricted/restricted.component';
import { ValuesService } from './values.service';


import { IsLoggedInGuard } from './is-logged-in.guard';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RestrictedComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    AuthenticationService,
    ValuesService,
    IsLoggedInGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
