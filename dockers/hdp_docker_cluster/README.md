# Docker based Multi Node HDP Cluster

### Preface

* This is a docker based cluster setup utility for Linux/Mac based OS. This is a shell script based solution to automate creation of a dockerized cluster with added option of installing Hortonworks Data Platform. The script is responsible for creating centos7 based Base images from which containers will be spawned. The script will install containers named namenode and kerbldap along with n number of datanodes that you specify. The containers are identical except that some services are designated for a specific container and named accordingly. For example kerbldap has additional kerberos and ldap service running, whereas namenode has a mysql server required for ambari,hive,ranger etc.

| Docker Containers          | Services|
|----------------------------|------------------------------------------------------------------------------------------------------------------------|
| Common Servies & Utilities | ssh, telnet, jdk 1.8, ntp, systemd, rsyslog, httpd, ssl, kerberos client, sssd, authconfig, network,wget, IST Timezone|
| kerbldap                   | kerberos server, kadmin server, openldap server|
| namenode                   | mysql server|
| datanode[1-n]              | kerbldap and namenode can password ssh datanodes|

This can be an ideal platform to have hands on with hadoop installation, High Availability configurations, ldap authentication, kerberos etc.


### Pre-Requisites 

* Docker Software
* Java SDK RPM
* JCE (Java Cryptography Extension)
* HDP, HDP-UTILS & Ambari Packages


### Cluster Installation
* Clone Repo or download Repo and extract the zip.
* Place Java JDK RPM and JCE( Java Cryptography Extension) jce_policy-*.zip inside RPMS folder
* run masterscript.sh and select option 1 to install the cluster.
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
