#!/bin/bash

domain_name=$1
kerbldap_hostname=$2
kerbldap_container=$3


docker exec -t $kerbldap_container bash -c "echo '# Configuration snippets may be placed in this directory as well
includedir /etc/krb5.conf.d/

[logging]
 default = FILE:/var/log/krb5libs.log
 kdc = FILE:/var/log/krb5kdc.log
 admin_server = FILE:/var/log/kadmind.log

[libdefaults]
 dns_lookup_realm = false
 ticket_lifetime = 24h
 renew_lifetime = 7d
 forwardable = true
 rdns = false
 default_realm = ${domain_name^^}
 default_ccache_name = KEYRING:persistent:%{uid}

[realms]
 ${domain_name^^} = {
 kdc = $kerbldap_hostname
 admin_server = $kerbldap_hostname
}

[domain_realm]
# .example.com = EXAMPLE.COM
# example.com = EXAMPLE.COM' > /etc/krb5.conf"