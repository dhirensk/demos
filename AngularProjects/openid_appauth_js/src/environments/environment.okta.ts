// src/environments/environment.ts
export const environment = {
  production: false,
  oauthConfig: {
    issuer: 'https://dev-13511724.okta.com/oauth2/ausfg0h3ttKW1i4R45d7',
    client_id: '0oafg0e3qhPkjd43y5d7',
    redirect_uri: 'http://localhost:4200/callback', // Adjust the redirect URI as needed
    scopes: "email profile openid",
    client_secret: 'not needed in authorization code with pkce. do not configure a client with secret', // Optional for public clients
    pfidpadapterid: 'HTMLFormSimplePCV'
  }
};