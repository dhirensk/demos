const express = require('express');
const OktaJwtVerifier = require('@okta/jwt-verifier');
const app = express();

const bodyParser = require('body-parser');

const expectedAud = "AngularSPA"
const expectClientid = "0oa64hdgz2uKuv80c5d7"
const nonce = "this_is_a_test_nonce_999999"
const oktaJwtVerifier = new OktaJwtVerifier({
  issuer: 'https://dev-45061733.okta.com/oauth2/aus64hhbp70XUaSIN5d7' // issuer required
});

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))

app.use((req, res, next) => {
  console.log("Hello from Middleware1");
  res.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  res.setHeader("Access-Control-Allow-Origin", "http://localhost:4200");
  next();
})

app.post('/verify/idtoken', (req, res) => {
  console.log("start idtoken verification");
  const idtoken = req.body.idtoken;
  if (!idtoken) {
    res.status(400).send('Bad Request. idtoken not received in the request')
  }
  console.log(idtoken);
  console.log(expectClientid);
  oktaJwtVerifier.verifyIdToken(idtoken, expectClientid, nonce).then(jwt => {
    // the token is valid
    console.log({ "verified_idToken": jwt.claims });
    res.status(200).json(jwt.claims)
  }).catch(err => {
    console.log(err)
    res.status(403).json({ err })
  });
});

app.post('/verify/accesstoken', (req, res) => {
  console.log("start accesstoken verification");
  const accesstoken = req.body.accesstoken;
  if (!accesstoken) {
    res.status(400).send('Bad Request. accesstoken not received in the request')
  }
  else {
    token = accesstoken
  }
  oktaJwtVerifier.verifyAccessToken(token, expectedAud).then(jwt => {
    // the token is valid
    console.log({ "verified_accesstoken": jwt.claims });
    res.status(200).json(jwt.claims)
  }).catch(err => {
    // a validation failed, inspect the error
    console.log(err)
    res.status(403).json({ err })
  });
});

module.exports = app;
