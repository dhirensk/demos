#! /bin/sh

if [ -z "$1" ]; then CERTS_DIR="$HOME"/certs_tutorial/nginx; else CERTS_DIR="$1"; fi
if [ -z "$2" ]; then PASS="test123"; else PASS="$2"; fi
if [ -z "$3" ]; then PASSWORD_FILE="global.pass"; else PASSWORD_FILE="$3"; fi

###  setup pre-requisites
rm -rf "$CERTS_DIR"
mkdir -p "$CERTS_DIR"/demoCA/newcerts
touch "$CERTS_DIR"/demoCA/index.txt
echo "$PASS" > "$CERTS_DIR"/"$PASSWORD_FILE"
cd "$CERTS_DIR"

###  Self-signed CA
openssl req -x509 -sha256 -newkey rsa:4096 -keyout "$CERTS_DIR"/ca_key.pem -passout pass:"$PASS" \
-days 3650 -out "$CERTS_DIR"/CACert.pem \
-subj "/C=IN/ST=MH/L=MU/O=HOME/CN=nginxCA"

### Server CSR
openssl req -new -newkey rsa:4096 -passout pass:"$PASS" \
-keyout "$CERTS_DIR"/s_private_key.pem -out "$CERTS_DIR"/server.csr \
-subj "/C=IN/ST=MH/L=MU/O=HOME/CN=nginxserver"

### V3_ca extensions for server certificate
cat <<EOF > server_v3_ca.cnf
[v3_ca]
basicConstraints = CA:FALSE
subjectKeyIdentifier = hash
authorityKeyIdentifier = keyid,issuer:always
keyUsage = critical, digitalSignature, keyEncipherment, keyCertSign
extendedKeyUsage = serverAuth
subjectAltName = @alt_names
[alt_names]
DNS.1 = nodejs.testserver.com
EOF

### create the server certificate
openssl ca -keyfile "$CERTS_DIR"/ca_key.pem -cert "$CERTS_DIR"/CACert.pem -passin pass:"$PASS" \
-in "$CERTS_DIR"/server.csr \
-out "$CERTS_DIR"/server.pem -days 1000 \
-extensions v3_ca -extfile "$CERTS_DIR"/server_v3_ca.cnf \
-rand_serial -notext -batch

###client

openssl req -new -newkey rsa:4096 -passout pass:"$PASS" \
-keyout "$CERTS_DIR"/c_private_key.pem -out "$CERTS_DIR"/client.csr \
-subj "/C=IN/ST=MH/L=MU/O=HOME/CN=client"

### V3_ca for client certificate
cat <<EOF > client_v3_ca.cnf
[v3_ca]
basicConstraints = CA:FALSE
subjectKeyIdentifier = hash
authorityKeyIdentifier = keyid,issuer:always
keyUsage = critical, digitalSignature, keyEncipherment, keyCertSign
extendedKeyUsage = clientAuth
subjectAltName = @alt_names
[alt_names]
email.1 = client@testserver.com
EOF

### create the client certificate
openssl ca -keyfile "$CERTS_DIR"/ca_key.pem -cert "$CERTS_DIR"/CACert.pem -passin pass:"$PASS" \
-in "$CERTS_DIR"/client.csr \
-out "$CERTS_DIR"/client.pem -days 1000 \
-extensions v3_ca -extfile "$CERTS_DIR"/client_v3_ca.cnf \
-rand_serial -notext -batch

### package the CA cert
openssl crl2pkcs7 -nocrl -certfile "$CERTS_DIR"/CACert.pem -out "$CERTS_DIR"/nginxCACert.p7b

### generate client keystore
openssl pkcs12 -export -out "$CERTS_DIR"/client.p12 -inkey "$CERTS_DIR"/c_private_key.pem -in "$CERTS_DIR"/client.pem -certfile "$CERTS_DIR"/CACert.pem -passin pass:"$PASS" -passout pass:"$PASS"