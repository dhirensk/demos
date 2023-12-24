namenode=$1
namenode_hostname=$(docker exec -t $namenode bash -c 'hostname' | tr -d '\r')

docker exec -t $namenode mysqladmin -u root password root
docker exec -t $namenode mysqladmin -u root 2>&1 >/dev/null
docker exec -t $namenode mysql -u root -proot -e "CREATE USER 'dbuser'@'localhost' IDENTIFIED BY 'dbuser';
GRANT ALL PRIVILEGES ON *.* TO 'dbuser'@'localhost' IDENTIFIED BY 'dbuser'WITH GRANT OPTION;
CREATE USER 'dbuser'@'%' IDENTIFIED BY 'dbuser';
GRANT ALL PRIVILEGES ON *.* TO 'dbuser'@'%' IDENTIFIED BY 'dbuser' WITH GRANT OPTION;
CREATE USER 'dbuser'@'${namenode_hostname}' IDENTIFIED BY 'dbuser';
GRANT ALL PRIVILEGES ON *.* TO 'dbuser'@'${namenode_hostname}' IDENTIFIED BY 'dbuser' WITH GRANT OPTION;
FLUSH PRIVILEGES;
CREATE DATABASE ambari;"

docker exec -t $namenode mysql -u root -proot -e "CREATE USER 'hive'@'localhost' IDENTIFIED BY 'hive';
GRANT ALL PRIVILEGES ON *.* TO 'hive'@'localhost';
CREATE USER 'hive'@'%' IDENTIFIED BY 'hive';
GRANT ALL PRIVILEGES ON *.* TO 'hive'@'%';
CREATE USER 'hive'@'${namenode_hostname}' IDENTIFIED BY 'hive';
GRANT ALL PRIVILEGES ON *.* TO 'hive'@'${namenode_hostname}';
FLUSH PRIVILEGES;
CREATE DATABASE hive;"

docker exec -t $namenode mysql -u root -proot -e "CREATE USER 'rangerdba'@'localhost' IDENTIFIED BY 'rangerdba';
GRANT ALL PRIVILEGES ON *.* TO 'rangerdba'@'localhost' WITH GRANT OPTION;
CREATE USER 'rangerdba'@'%' IDENTIFIED BY 'rangerdba';
GRANT ALL PRIVILEGES ON *.* TO 'rangerdba'@'%' WITH GRANT OPTION;
CREATE USER 'rangerdba'@'${namenode_hostname}' IDENTIFIED BY 'rangerdba';
GRANT ALL PRIVILEGES ON *.* TO 'rangerdba'@'${namenode_hostname}' WITH GRANT OPTION;
FLUSH PRIVILEGES;"

docker exec -t $namenode mysql -u root -proot -e "CREATE USER 'oozie'@'localhost' IDENTIFIED BY 'oozie';
GRANT ALL PRIVILEGES ON *.* TO 'oozie'@'localhost' WITH GRANT OPTION;
CREATE USER 'oozie'@'%' IDENTIFIED BY 'oozie';
GRANT ALL PRIVILEGES ON *.* TO 'oozie'@'%' WITH GRANT OPTION;
CREATE USER 'oozie'@'${namenode_hostname}' IDENTIFIED BY 'oozie';
GRANT ALL PRIVILEGES ON *.* TO 'oozie'@'${namenode_hostname}' WITH GRANT OPTION;
FLUSH PRIVILEGES;
CREATE DATABASE oozie;"

docker exec -t $namenode mysql -u root -proot -e "SET GLOBAL log_bin_trust_function_creators = 1;"


docker exec -t $namenode bash -c "echo '#VERSION_NUMBER=2.5.2.0-298
[ambari-2.5.2.0]
name=ambari Version - ambari-2.5.2.0
baseurl=http://${namenode_hostname}/ambari/centos7
gpgcheck=0
enabled=1
priority=1' > /etc/yum.repos.d/ambari.repo"

docker exec -t $namenode bash -c "echo '#VERSION_NUMBER=2.6.2.0-205
[HDP-2.6.2.0]
name=HDP Version - HDP-2.6.2.0
baseurl=http://${namenode_hostname}/HDP/centos7
gpgcheck=0
enabled=1
priority=1


[HDP-UTILS-1.1.0.21]
name=HDP-UTILS Version - HDP-UTILS-1.1.0.21
baseurl=http://${namenode_hostname}/HDP-UTILS/
gpgcheck=0
enabled=1
priority=1' > /etc/yum.repos.d/hdp.repo"


docker exec -t $namenode chown -R root:root /etc/yum.repos.d/
docker exec -t $namenode mkdir '/var/www/html/HDP-UTILS'
#docker exec -t namenode bash -c 'tar -xvf /Downloads/HDP-2.6.2.0-centos7-rpm.tar.gz -C /var/www/html/'
#docker exec -t namenode bash -c 'tar -xvf /Downloads/HDP-UTILS-1.1.0.21-centos7.tar -C /var/www/html/HDP-UTILS/'
#docker exec -t namenode bash -c 'tar -xvf /Downloads/ambari-2.5.2.0-centos7.tar -C /var/www/html/'
docker exec -t $namenode bash -c 'tar -xvf /Downloads/HDP-2.6.*-rpm.tar.gz -C /var/www/html/'
docker exec -t $namenode bash -c 'tar -xvf /Downloads/HDP-UTILS*.tar -C /var/www/html/HDP-UTILS/'
docker exec -t $namenode bash -c 'tar -xvf /Downloads/ambari-2.5.*.tar -C /var/www/html/'
docker exec -t $namenode yum install -y ambari-server
docker exec -t $namenode mysql -u dbuser -pdbuser -e "use ambari;
source /var/lib/ambari-server/resources/Ambari-DDL-MySQL-CREATE.sql;"
# copy the ssh key to the host required for ambari hosts registration
docker container cp $namenode:/root/.ssh/id_rsa ~/Downloads/
docker exec -t $namenode bash -c 'ambari-server setup --jdbc-db=mysql --jdbc-driver=/usr/share/java/mysql-connector-java.jar'
docker exec -it $namenode bash -c 'ambari-server setup'
docker exec -t $namenode bash -c 'ambari-server start'
