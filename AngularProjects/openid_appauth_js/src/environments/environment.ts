// src/environments/environment.ts
export const environment = {
  production: false,
  oauthConfig: {
    issuer: 'https://172.15.0.11:9031',
    client_id: 'sso-client',
    redirect_uri: 'http://localhost:4200/callback', // Adjust the redirect URI as needed
    scopes: "email profile openid",
    client_secret: 'not needed in authorization code with pkce. do not configure a client with secret', // Optional for public clients
    pfidpadapterid: 'HTMLFormSimplePCV'
  }
};
