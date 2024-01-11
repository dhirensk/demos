#!/bin/bash

container_name=$1
domain_name=$2
domain_name_upper="${domain_name^^}"
hostname=$3
echo "

<======================================== Configuring LDAP on $hostname ========================================>"
domain=$(echo $domain_name | sed 's/\(.*\)\..*/\1/') #return first part of domain name

#echo "test1"
# print a sample ldap config change file and update the hash of password "admin"
docker exec -t $container_name bash -c "echo -n 'dn: olcDatabase={2}hdb,cn=config
changetype: modify
replace: olcSuffix
olcSuffix: dc=${domain},dc=com

dn: olcDatabase={2}hdb,cn=config
changetype: modify
replace: olcRootDN
olcRootDN: cn=admin,dc=${domain},dc=com

dn: olcDatabase={2}hdb,cn=config
changetype: modify
replace: olcRootPW
olcRootPW: ' > /tmp/db.ldif"

#echo "test2"
#docker exec -d $container_name bash -c 'systemctl status slapd | grep running'
while true; do
	sleep 1
	docker exec -t $container_name bash -c 'systemctl status slapd | grep running'
	if [ $? -eq 0 ]; then
		# shell variable will not be executed in current shell since everything is enclosed in single quotes.
		break
	fi
	# waiting for ldap services to start
done
	
docker exec -t $container_name bash -c 'echo -e "$(slappasswd -s admin)" >> /tmp/db.ldif'
docker exec -t $container_name bash -c 'ldapmodify -Y EXTERNAL -H ldapi:/// -f /tmp/db.ldif'
docker exec -t $container_name bash -c 'cp /usr/share/openldap-servers/DB_CONFIG.example /var/lib/ldap/DB_CONFIG'
docker exec -t $container_name bash -c 'ldapadd -Y EXTERNAL -H ldapi:/// -f /etc/openldap/schema/cosine.ldif'
docker exec -t $container_name bash -c 'ldapadd -Y EXTERNAL -H ldapi:/// -f /etc/openldap/schema/nis.ldif'
docker exec -t $container_name bash -c 'ldapadd -Y EXTERNAL -H ldapi:/// -f /etc/openldap/schema/inetorgperson.ldif'
#echo "test3"
docker exec -t $container_name bash -c "echo 'dn: dc=${domain},dc=com
dc: ${domain}
objectClass: top
objectClass: domain

dn: cn=admin,dc=${domain},dc=com
objectClass: organizationalRole
cn: admin
description: LDAP Manager

dn: ou=People,dc=${domain},dc=com
objectClass: organizationalUnit
ou: People

dn: ou=Groups,dc=${domain},dc=com
objectClass: organizationalUnit
ou: Groups' > /tmp/base.ldif"
#echo "test4"
docker exec -t $container_name bash -c "ldapadd -x -w 'admin' -D 'cn=admin,dc=${domain},dc=com' -f /tmp/base.ldif"

		#Create an ldap user with shadow password
		#docker exec -t kerbldap bash -c 'mkdir -p /users/dhiren'
		#docker exec -t kerbldap bash -c 'useradd -b /users/dhiren/ -s /sbin/nologin -u 1000 -U dhiren'
#echo "test5"
docker exec -t $container_name bash -c "echo '# UPG User Private Group 
dn: cn=dhiren,ou=Groups,dc=${domain},dc=com
cn: dhiren
gidNumber: 1000
objectclass: top
objectclass: posixGroup

# User dhiren
dn: uid=dhiren,ou=People,dc=${domain},dc=com
cn: Dhirendra Khanka
givenName: Dhirendra
sn: Khanka
uid: dhiren
uidNumber: 1000
gidNumber: 1000
homeDirectory: /users/dhiren
loginShell: /bin/bash
mail: dhirensk@gmail.com
objectClass: top
objectClass: inetOrgPerson
objectClass: posixAccount
objectClass: shadowAccount
userPassword: {SSHA}qVqoCanFjVTALoyC7TL8tuppWCfRUSJV' > /tmp/dhiren.ldif"

docker exec -t $container_name bash -c "echo '
dn: cn=developers,ou=Groups,dc=${domain},dc=com
objectClass: groupOfNames
objectClass: top
cn: developers
member: uid=devuser1,ou=People,dc=${domain},dc=com
member: uid=devuser2,ou=People,dc=${domain},dc=com

dn: uid=devuser1,ou=People,dc=${domain},dc=com
objectClass: inetOrgPerson
objectClass: posixAccount
objectClass: shadowAccount
objectClass: top
cn: developer user1
gidNumber: 1000
homeDirectory: /users/devuser1
sn: devuser
uid: devuser1
uidNumber: 1200
loginShell: /bin/bash
userPassword:: ZGV2dXNlcjE=

dn: uid=devuser2,ou=People,dc=${domain},dc=com
objectClass: inetOrgPerson
objectClass: posixAccount
objectClass: shadowAccount
objectClass: top
cn: developer user2
gidNumber: 1000
homeDirectory: /users/devuser2
sn: devuser
uid: devuser2
uidNumber: 1201
loginShell: /bin/bash
userPassword:: ZGV2dXNlcjI=' > /tmp/users.ldif"

#echo "test6"
docker exec -t $container_name bash -c "ldapadd -x -w admin -H ldap://${hostname} -D 'cn=admin,dc=${domain},dc=com' -f /tmp/dhiren.ldif"
docker exec -t $container_name bash -c "ldapadd -x -w admin -H ldap://${hostname} -D 'cn=admin,dc=${domain},dc=com' -f /tmp/users.ldif"
docker exec -t $container_name bash -c "ldappasswd -x -w admin -s dhiren -D 'cn=admin,dc=${domain},dc=com' 'uid=dhiren,ou=People,dc=${domain},dc=com'"

docker exec -t $container_name bash -c 'touch /etc/pki/CA/index.txt; echo 01 > /etc/pki/CA/serial'
docker exec -t $container_name bash -c "openssl req -verbose -new -x509 -nodes \
-out /etc/openldap/cacerts/CA.crt \
-keyout /etc/pki/CA/private/CA.key \
-days 1000 -subj '/C=IN/ST=MH/L=MU/O=TD/OU=MS/CN=dhirendra Khanka'"

docker exec -t $container_name bash -c "openssl req -verbose -nodes -newkey rsa:4096 \
-keyout /etc/openldap/certs/server.key \
-out /etc/pki/CA/certs/server.csr \
-subj '/C=IN/ST=MH/L=MU/O=TD/OU=MS/CN=${hostname}'"

docker exec -t $container_name bash -c 'openssl ca -keyfile /etc/pki/CA/private/CA.key \
-cert /etc/openldap/cacerts/CA.crt \
-in /etc/pki/CA/certs/server.csr \
-out /etc/openldap/certs/server.crt -days 1000 -batch'

docker exec -t $container_name bash -c 'chown -R ldap:ldap /etc/openldap/certs'
		#docker exec -t $container_name bash -c 'chmod 600 /etc/openldap/certs/server.key'
docker exec -t $container_name bash -c 'systemctl restart slapd'
while true; do
	sleep 1
	docker exec -t $container_name bash -c 'systemctl status slapd | grep running'
	if [ $? -eq 0 ]; then
	break
	fi
done
		

docker exec -t $container_name bash -c 'echo "dn: cn=config
changetype: modify
add: olcTLSCACertificateFile
olcTLSCACertificateFile: /etc/openldap/cacerts/CA.crt
-
replace: olcTLSCertificateKeyFile
olcTLSCertificateKeyFile: /etc/openldap/certs/server.key
-
replace: olcTLSCertificateFile
olcTLSCertificateFile: /etc/openldap/certs/server.crt" > /tmp/certs.ldif'
#echo "test7"
docker exec -t $container_name bash -c 'ldapmodify -Y EXTERNAL -H ldapi:/// -f /tmp/certs.ldif'
		# enable ldaps:///
docker exec -t $container_name bash -c 'slapcat -b "cn=config" | egrep "olcTLSCertificateFile|olcTLSCertificateKeyFile|olcTLSCACertificateFile"'
docker exec -t $container_name bash -c 'sed -i "s/SLAPD_URLS.*/SLAPD_URLS=\"ldapi:\/\/\/ ldap:\/\/\/ ldaps:\/\/\/\"/g" /etc/sysconfig/slapd'
docker exec -t $container_name bash -c 'systemctl restart slapd'
		#docker exec -t kerbldap bash -c 'sed -i "/TLS_CACERTDIR/s/^/#/g" /etc/openldap/ldap.conf'
docker exec -t $container_name bash -c 'echo "TLS_CACERT /etc/openldap/cacerts/CA.crt" >> /etc/openldap/ldap.conf'
docker exec -t $container_name bash -c "authconfig --enableldap --enableldapauth \\
--ldapserver=$hostname --ldapbasedn='dc=$domain,dc=com' --enablesssd \\
--enablesssdauth --enableldaptls --enablemkhomedir --disablecache --disablecachecreds --update"

echo "LDAP Configured Successfully on $hostname"
#Configuring Kerberos 

echo "

<======================================== Configuring Kerberos on $hostname ===========================================>"
#Adding kdc.conf file
docker exec -t $container_name bash -c "echo '[kdcdefaults]
 kdc_ports = 88
 kdc_tcp_ports = 88

[realms]
 $domain_name_upper = {
  #master_key_type = aes256-cts
  acl_file = /var/kerberos/krb5kdc/kadm5.acl
  dict_file = /usr/share/dict/words
  admin_keytab = /var/kerberos/krb5kdc/kadm5.keytab
  supported_enctypes = aes256-cts:normal aes128-cts:normal des3-hmac-sha1:normal arcfour-hmac:normal camellia256-cts:normal camellia128-cts:normal des-hmac-sha1:normal des-cbc-md5:normal des-cbc-crc:normal
 }
' > /etc/kdc.conf"
docker exec -t $container_name cp /etc/kdc.conf /var/kerberos/krb5kdc/
# supply master kdc password and reconfirm password
docker exec -t $container_name bash -c "echo -e 'root\nroot' | kdb5_util create"
docker exec -t "$container_name" systemctl enable krb5kdc
docker exec -t "$container_name" systemctl start krb5kdc
docker exec -t "$container_name" systemctl enable kadmin
docker exec -t "$container_name" systemctl start kadmin
# add admin pricipal and provide admin as password, also reconfirm admin password
docker exec -t $container_name bash -c "echo -e 'admin\nadmin' | kadmin.local -q 'addprinc admin/admin'"
docker exec -t $container_name bash -c "echo -e '*/admin@${domain_name_upper} *' > /var/kerberos/krb5kdc/kadm5.acl"
echo "Kerberos Server Configured Successfully on $hostname"

