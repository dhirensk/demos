# Authorization Code flow with PKCE using @openid/appauth

### Description

This project demonstrates Authorization code Flow with PKCE using @openid/appauth npm library. This repo can be used to test Oauth PKCE flow against any authorization server such as PingFederate, OKTA, KeyCloak etc

* Client - Angular
* Authorization Server - Okta/PingFederate etc. 
* Note: This demo does not include any resource server. The minimal requirements for a Resource server would be to validate the incoming incoming token in the request via introspect and map the claims to appropriate resource and send the requested resource to the client with required CORS Response headers.

### PingFederate Authorization Server
#### User login

<p align="center"> <img  src="src/assets/ping_login.png?raw=true"> </p>

#### User Consent

<p align="center"> <img  src="src/assets/ping_consent.png?raw=true"> </p>

#### Redirect
<p align="center"> <img  src="src/assets/ping_redirect.png?raw=true"> </p>


### Okta Authorization Server
#### User login

<p align="center"> <img  src="src/assets/okta_login.png?raw=true"> </p>

#### User Consent

<p align="center"> <img  src="src/assets/okta_consent.png?raw=true"> </p>

#### Redirect
<p align="center"> <img  src="src/assets/okta_redirect.png?raw=true"> </p>