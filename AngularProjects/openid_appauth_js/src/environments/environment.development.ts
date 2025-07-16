// src/environments/environment.ts
export const environment = {
  production: false,
  oauthConfig: {
    issuer: 'http://172.15.0.11:9030',
    client_id: 'sso-client',
    redirect_uri: 'http://localhost:4200/callback', // Adjust the redirect URI as needed
    scopes: "email profile openid",
    client_secret: '5dhXn6J8MwO9dDNU4OXdXirCemiPhqv6daKl4dQ2jyJpaxGbeSv2sb1CdY0BaX5Z' // Optional for public clients
  }
};