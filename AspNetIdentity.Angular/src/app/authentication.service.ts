import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { JwtHelper } from 'angular2-jwt';

import { AuthUser, JwtToken } from './auth-user';

import { environment } from '../environments/environment';

const CurrentUserTokenKey = 'CURRENT_USER';

@Injectable()
export class AuthenticationService {
  private loginUrl = environment.apiHost.replace(/\/$/, '') + '/auth/Token';
  private jwtHelper: JwtHelper = new JwtHelper();
  private loginChangedSource = new Subject<boolean>();
  public onLoginChanged: Observable<boolean> = this.loginChangedSource.asObservable();
  public currentUser: AuthUser = null;
  public token: string;

  constructor(private http: HttpClient) {
    // set token and current user if saved in local storage
    this.currentUser = this.getStoredUser();
    this.token = !!this.currentUser && this.currentUser.access_token;
  }

  public login(username: string, password: string): Observable<AuthUser> {
    const params = new HttpParams()
                      .set('username', username)
                      .set('password', password)
                      .set('grant_type', 'password')
                      .set('client_id', environment.jwtTokenConfig.client_id);

    return this.sendRequest(params);
  }

  public logout(): void {
    this.token = null;
    this.currentUser = null;
    localStorage.removeItem(CurrentUserTokenKey);

    this.loginChangedSource.next(false);
  }

  public refreshToken(): Observable<AuthUser> {
    const user = this.getStoredUser();
    if (!user) {
        throw new Error('Token does not exist');
    }

    const params = new HttpParams()
                      .set('refresh_token', user.refresh_token)
                      .set('grant_type', 'refresh_token')
                      .set('client_id', environment.jwtTokenConfig.client_id);

    return this.sendRequest(params);
  }

  public get isLoggedIn(): boolean {
    return !!this.token && !this.jwtHelper.isTokenExpired(this.token);
  }

  public getStoredUser(): AuthUser {
    return JSON.parse(localStorage.getItem(CurrentUserTokenKey)) as AuthUser;
  }

  private sendRequest(params: HttpParams): Observable<AuthUser> {
      const body: string = params.toString();

      return this.http
        .post(this.loginUrl, body, {
            headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
         })
         .catch(this.handleError)
         .map((response: JwtToken) => this.saveToken(response))
  }

  private saveToken(response: JwtToken): AuthUser {
    // login successful if there's a jwt token in the response
    const token = response && response.access_token;
    if (token) {
        // set token property
        this.token = token;
        this.currentUser = new AuthUser(this.jwtHelper.decodeToken(token), response.access_token, response.refresh_token);

        // store user and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem(CurrentUserTokenKey, JSON.stringify(this.currentUser));
        this.loginChangedSource.next(true);
        return this.currentUser;
    } else {
        this.loginChangedSource.next(false);
        return null;
    }
  }

  private handleError = (err: any) => Observable.throw(err.error || 'Server error');
}
