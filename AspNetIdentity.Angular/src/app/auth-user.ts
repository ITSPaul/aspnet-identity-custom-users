const RoleClaimTypeFieldName = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export class AuthUser {

    id: number;
    username: string;
    firstName: string;
    lastName: string;
    access_token: string;
    refresh_token: string;
    roles: string[] = [];

    constructor(user: any, access_token: string, refresh_token: string) {
        if (user) {
          this.id = user.UserId;
          this.username = user.UserName;
          this.firstName = user.FirstName;
          this.lastName = user.LastName;
          this.access_token = access_token;
          this.refresh_token = refresh_token;

          if (user[RoleClaimTypeFieldName]) {
            this.roles = [].concat(user[RoleClaimTypeFieldName]);
          }
        }
    }

    public hasRole(role: string): boolean {
      return !!this.roles && this.roles.indexOf(role) >= 0;
    }
}

export interface JwtToken {
  access_token: string;
  token_type: string;
  expires_in: string;
  refresh_token: string;
}
