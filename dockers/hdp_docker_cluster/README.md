# Docker based Multi Node HDP Cluster

### Preface

* This is a docker based cluster setup utility for Linux/Mac based OS. This is a shell script based solution to automate creation of a dockerized cluster with added option of installing Hortonworks Data Platform. Centos7 docker image is used as base for the dockerfile. A masterscript is provided for configuring the cluster setup.

| Docker Containers          | Services|
|----------------------------|------------------------------------------------------------------------------------------------------------------------|
| Common Servies & Utilities | ssh, telnet, jdk 1.8, ntp, systemd, rsyslog, httpd, ssl, kerberos client, sssd, authconfig, network,wget, IST Timezone|
| namenode                   | mysql server, kerberos server, kadmin server, openldap server |
| datanode[1-n]              | kerbldap and namenode can password ssh datanodes|



### Pre-Requisites 

* Docker Software
* Java JDK RPM
* JCE (Java Cryptography Extension)
* HDP, HDP-UTILS & Ambari Packages


### Cluster Installation
* Clone Repo or download Repo and extract the zip.
* Place Java JDK RPM inside RPMS folder
* run sudo bash masterscript.sh and select option 1 to install the cluster.
* enter the number of datanodes to be setup e.g. 3

### HDP Installation(mysql backend)

* The host machine's ~/Downloads directory is shared with the container in read-write mode.   
To mount the directory as read-only change the ```docker run -v ~/Downloads:/Downloads:ro```
To setup Yum local Repo for HDP installation, you need to download the relevant packages and repo files and place it in your ~/Downloads folder.  
[hdp 2.6 Repo](https://docs.hortonworks.com/HDPDocuments/Ambari-2.5.2.0/bk_ambari-installation/content/hdp_26_repositories.html)  
[Ambari 2.5 Repo](https://docs.hortonworks.com/HDPDocuments/Ambari-2.5.2.0/bk_ambari-installation/content/ambari_repositories.html)  
~/Downloads/HDP-2.6.2.0-centos7-rpm.tar.gz  
~/Downloads/HDP-UTILS-1.1.0.21-centos7.tar  
~/Downloads/ambari-2.5.2.0-centos7.tar  

* Update the repo files ambari.repo & hdp.repo accordingly as per your version of hdp package. Remove gpgcheck validation and repoint the baseurl to the namenode container. 

* run the masterscript.sh and choose- install Hortonworks Data Platform.

* MYSQL schemas ambari and hive have been already created. 
* Below DBA user accounts have been created.

| User | Password |
|------|----------|
| root | root     |
| dbusder|dbuser|
| hive | hive   |
| rangerdba | rangerdba | 

You will be required to enter information during ambari server setup. Once setup is complete, the server will be started.

login to ambari from host machine using localhost:8081
Due to port conflicts with Mac host's services, some of the exposed ports are remapped as below.  

|Container| Host|
|--------|-----|
|1221|1220|
|2050|2049|
|8006|8005|
|8081|8080|
|8089|8088|
|8444|8443|

        *******************************************************************************

        dhiren@DSKHOME:~/demos/dockers/hdp_docker_cluster$ sudo bash masterscript.sh 

        >>>>>> Docker Based HDP Cluster <<<<<<


        Enter number from below Actions
        1. Install Docker Images
        2. Container/Cluster Initialization
        3. Show all Images and Containers
        4. Start Cluster
        5. Stop Cluster
        6. Delete Cluster
        7. Configure mysqld on primary masternode
        8. Install Hortonworks Data Platform
        0. Exit

        Enter your choice : 2

        Docker container/cluster initialization?  yes/no : yes

        >>>>>> Cluster Installation <<<<<<

        Enter the name of your cluster : test
        
        Enter Number of master nodes: 1

        Enter number of datanodes to be setup.
        Type 0 or press enter to skip datanodes : 1
        
        Enter Domain name e.g. mydocker.com 
        Press enter to use a default domain (dkdocker.com) : 

        Provide network subnet to use for this cluster 
        Press enter to use a default subnet 172.16.0.0/24.
        Please do not use 172.17.0.0/24 as it is assigned to default bridge: 
        64c26647571bb1286c0fdaadfe298557c8b50652fdac92145e6d78fd35599d42

        Enter the masternode hostname prefix  
        e.g. using masternode as prefix will create masternode1, masternode2 ... 
        press enter for default (masternode): 

        Enter the datanode hostname prefix 
        e.g  using datanode as prefix will create datanode1, datanode2 ...
        press enter for default (datanode) : 

        Install kerberos and ldap yes/no : yes
        Kerberos and ldap will be installed on last masternode


        <====== CONFIGURATION SUMMARY ======>

        Total number of Master Nodes         : 1
        Total number of Data Nodes           : 1
        Total number of Nodes in the cluster : 2
        Domain Name                          : dkdocker.com
        Cluster Name                         : test
        Masternodes Prefix                   : masternode
        Datanodes Prefix                     : datanode
        Install Kerberos and LDAP Services   : yes
        Network Subnet                       : 172.16.0.0/24

        Continue yes/no ?yes

        <====== Initializing Masternodes ======>

        Initializing container for ======> masternode1

        e14afc823befc2446b82c5a6f327dc6fc8a7e926a5742f54bf563520d8535a0c
        Configuring Services on container ======> test_masternode1
        Configured following services on the container =====> httpd,java,jce,syslog,sshd
        waiting for system to be ready
        Adding all masternode entries to ======> /etc/hosts
        Adding all datanode entries to ======> /etc/hosts
        Configuring network
        Created symlink from /etc/systemd/system/multi-user.target.wants/slapd.service to /usr/lib/systemd/system/slapd.service.


        <================= Configuring LDAP on masternode1.dkdocker.com =========================>
        Active: active (running) since Thu 2024-01-11 12:54:31 IST; 1s ago
        SASL/EXTERNAL authentication started
        SASL username: gidNumber=0+uidNumber=0,cn=peercred,cn=external,cn=auth
        SASL SSF: 0
        modifying entry "olcDatabase={2}hdb,cn=config"

        modifying entry "olcDatabase={2}hdb,cn=config"

        modifying entry "olcDatabase={2}hdb,cn=config"

        SASL/EXTERNAL authentication started
        SASL username: gidNumber=0+uidNumber=0,cn=peercred,cn=external,cn=auth
        SASL SSF: 0
        adding new entry "cn=cosine,cn=schema,cn=config"

        SASL/EXTERNAL authentication started
        SASL username: gidNumber=0+uidNumber=0,cn=peercred,cn=external,cn=auth
        SASL SSF: 0
        adding new entry "cn=nis,cn=schema,cn=config"

        SASL/EXTERNAL authentication started
        SASL username: gidNumber=0+uidNumber=0,cn=peercred,cn=external,cn=auth
        SASL SSF: 0
        adding new entry "cn=inetorgperson,cn=schema,cn=config"

        adding new entry "dc=dkdocker,dc=com"

        adding new entry "cn=admin,dc=dkdocker,dc=com"

        adding new entry "ou=People,dc=dkdocker,dc=com"

        adding new entry "ou=Groups,dc=dkdocker,dc=com"

        adding new entry "cn=dhiren,ou=Groups,dc=dkdocker,dc=com"

        adding new entry "uid=dhiren,ou=People,dc=dkdocker,dc=com"

        adding new entry "cn=developers,ou=Groups,dc=dkdocker,dc=com"

        adding new entry "uid=devuser1,ou=People,dc=dkdocker,dc=com"

        adding new entry "uid=devuser2,ou=People,dc=dkdocker,dc=com"

        Using configuration from /etc/pki/tls/openssl.cnf
        Generating a 2048 bit RSA private key
        .............................+++
        ........................................+++
        writing new private key to '/etc/pki/CA/private/CA.key'
        -----
        Using configuration from /etc/pki/tls/openssl.cnf
        Generating a 4096 bit RSA private key
        ........................................................++
        ..............................................................................................................++
        writing new private key to '/etc/openldap/certs/server.key'
        -----
        Using configuration from /etc/pki/tls/openssl.cnf
        Check that the request matches the signature
        Signature ok
        Certificate Details:
                Serial Number: 1 (0x1)
                Validity
                    Not Before: Jan 11 07:24:35 2024 GMT
                    Not After : Oct  7 07:24:35 2026 GMT
                Subject:
                    countryName               = IN
                    stateOrProvinceName       = MH
                    organizationName          = TD
                    organizationalUnitName    = MS
                    commonName                = masternode1.dkdocker.com
                X509v3 extensions:
                    X509v3 Basic Constraints: 
                        CA:FALSE
                    Netscape Comment: 
                        OpenSSL Generated Certificate
                    X509v3 Subject Key Identifier: 
                        40:17:5D:64:BF:2A:50:8E:2B:A9:18:32:5B:DA:8C:1D:A2:93:82:92
                    X509v3 Authority Key Identifier: 
                        keyid:9E:42:D4:92:67:97:BA:E4:1C:8D:A5:7C:EB:3F:48:C5:01:27:30:F0

        Certificate is to be certified until Oct  7 07:24:35 2026 GMT (1000 days)

        Write out database with 1 new entries
        Data Base Updated
        Active: active (running) since Thu 2024-01-11 12:54:35 IST; 1s ago
        SASL/EXTERNAL authentication started
        SASL username: gidNumber=0+uidNumber=0,cn=peercred,cn=external,cn=auth
        SASL SSF: 0
        modifying entry "cn=config"

        olcTLSCACertificateFile: /etc/openldap/cacerts/CA.crt
        olcTLSCertificateKeyFile: /etc/openldap/certs/server.key
        olcTLSCertificateFile: /etc/openldap/certs/server.crt
        getsebool:  SELinux is disabled
        LDAP Configured Successfully on masternode1.dkdocker.com


        <============= Configuring Kerberos on masternode1.dkdocker.com ====================>
        Loading random data
        Initializing database '/var/kerberos/krb5kdc/principal' for realm 'DKDOCKER.COM',
        master key name 'K/M@DKDOCKER.COM'
        You will be prompted for the database Master Password.
        It is important that you NOT FORGET this password.
        Enter KDC database master key: 
        Re-enter KDC database master key to verify: 
        Created symlink from /etc/systemd/system/multi-user.target.wants/krb5kdc.service to /usr/lib/systemd/system/krb5kdc.service.
        Created symlink from /etc/systemd/system/multi-user.target.wants/kadmin.service to /usr/lib/systemd/system/kadmin.service.
        Authenticating as principal root/admin@DKDOCKER.COM with password.
        WARNING: no policy specified for admin/admin@DKDOCKER.COM; defaulting to no policy
        Enter password for principal "admin/admin@DKDOCKER.COM": 
        Re-enter password for principal "admin/admin@DKDOCKER.COM": 
        Principal "admin/admin@DKDOCKER.COM" created.
        Kerberos Server Configured Successfully on masternode1.dkdocker.com
        Installing mysql
        moving any online yum repos to /root/
        Container test_masternode1 Initialization Successful
        ---------------------------------------------------
        <====== Initializing Datanodes ======>

        Initializing container for ======> datanode1
        da8f22a3818302ee18733403038453f40d446a75b597d4887ab96392dacdee64
        Configuring services on container ======> test_datanode1
        Configured following services on the container =====> httpd,java,jce,syslog,sshd
        waiting for system ready up
        Adding all masternode entries to ======> /etc/hosts
        Adding all datanode entries to ======> /etc/hosts
        Configuring network
        moving any online yum repos to /root/ 
        Container test_datanode1 Initialization Successful
        ---------------------------------------------------


        <=================== Configuring ssh passwordless for all nodes ======================>
        /usr/bin/ssh-copy-id: INFO: Source of key(s) to be installed: "/root/.ssh/id_rsa.pub"

        Number of key(s) added: 1

        Now try logging into the machine, with:   "ssh 'root@masternode1.dkdocker.com'"
        and check to make sure that only the key(s) you wanted were added.

        /usr/bin/ssh-copy-id: INFO: Source of key(s) to be installed: "/root/.ssh/id_rsa.pub"

        Number of key(s) added: 1

        Now try logging into the machine, with:   "ssh 'root@datanode1.dkdocker.com'"
        and check to make sure that only the key(s) you wanted were added.

        /usr/bin/ssh-copy-id: INFO: Source of key(s) to be installed: "/root/.ssh/id_rsa.pub"

        Number of key(s) added: 1

        Now try logging into the machine, with:   "ssh 'root@masternode1.dkdocker.com'"
        and check to make sure that only the key(s) you wanted were added.

        /usr/bin/ssh-copy-id: INFO: Source of key(s) to be installed: "/root/.ssh/id_rsa.pub"

        Number of key(s) added: 1

        Now try logging into the machine, with:   "ssh 'root@datanode1.dkdocker.com'"
        and check to make sure that only the key(s) you wanted were added.

        ssh passwordless configured Successfully on all nodes
        Configuring /etc/krb5.conf on rest of the nodes
        krb5.conf                                                                                                                100%  592   706.7KB/s   00:00    

        <================== Configuring LDAP Authentication on all Nodes ====================>
        CA.crt                                                                                                                   100% 1289     1.7MB/s   00:00    
        getsebool:  SELinux is disabled
        LDAP authentication successfully configured on test_datanode1
        NAMES              IMAGE        CREATED          STATUS
        test_datanode1     datanode     16 seconds ago   Up 15 seconds
        test_masternode1   masternode   39 seconds ago   Up 36 seconds




        -------------------------------------------------------------------------------
        ***************************IMPORTANT INFORMATION*******************************
        -------------------------------------------------------------------------------

        Common Servies & Utilities                      :ssh, telnet, jdk 1.8, ntp, systemd,
                                                        rsyslog, httpd, ssl, kerberos client, sssd,
                                                        authconfig, network,wget, IST Timezone
        test_masternode1   :kerberos server, kadmin server, openldap server
        test_masternode1            :mysql server, ambari server

        local user root password : root exists on all hosts
        ldap user dhiren password : dhiren exist on ldap

        to connect to any of the containers : 
        ssh root@localhost -p ####
        Container Port range from 2221:2222

        kerberos Admin principal : admin/admin@DKDOCKER.COM
        kadmin password : admin

        openldap admin DN : cn=admin,dc=dkdocker,dc=com
        admin password    : admin

        *******************************************************************************