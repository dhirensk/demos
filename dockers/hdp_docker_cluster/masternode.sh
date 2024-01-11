#!/bin/bash
current_master=$1
masters=$2
dnodes=$3
totalnodes=$4
domain_name=$5
clustername=$6
masternode_prefix=$7
datanode_prefix=$8
kerb_ldap=$9
subnet=${10}
install_kerbldap=0

echo ""
echo "Initializing container for ======> $masternode_prefix$current_master
"


. ./PortMappings # or source PortMappings

#container name format  HadoopDev_masternode1
container_name="$clustername"_"$masternode_prefix""$current_master"

#hostname format  masternode1.dkdocker.com
hostname="$masternode_prefix""$current_master"."$domain_name"
# returns 172.16.0. if the subnet is 172.16.0.0/24
networkid=$(echo $subnet | sed 's/\([0-9]*\.\)\([0-9]*\.\)\([0-9]*\.\).*/\1\2\3/')
hostip="$current_master"
hostipaddr="$networkid""$hostip"

#echo "test---------------------------------1"
docker ps -a | grep -q "$container_name"
if [ $? -eq 0 ]; then
	docker start "$container_name"
else

    #echo "test---------------------------------2"

	#Create a network bridge while creating the first masternode container for the cluster
	if [ "$current_master" == "1" ]; then

		#echo "test---------------------------------3"

		if [ "$current_master" == "$masters" ] && [ "$kerb_ldap" == "yes" ]; then
			install_kerbldap=1
			portmappings="$masternodeportskerberos"
		else
			portmappings="$masternodeports"
		fi

		#echo "test---------------------------------4"
		#echo $hostipaddr
		# important to NOT keep portmappings inside quotes
		#we add fqdn as hostname and also the hostname with same ip separately because docker is no longer splitting fqdn in hosts file
		# /etc/hosts
		#172.16.0.1      masternode1
        #172.16.0.1      masternode1.dkdocker.com
		#--add-host "$masternode_prefix""$current_master":"$hostipaddr" \

		docker run -i -t -d -v /sys/fs/cgroup:/sys/fs/cgroup:ro --tmpfs /run \
			-v ~/Downloads:/Downloads:ro \
			--name "$container_name" --hostname "$hostname" --privileged \
			-p 222"$current_master":22 \
			$portmappings \
			--ip "$hostipaddr" \
			--network "$clustername" \
			-e "JAVA_HOME=/usr/java/latest" \
			masternode

		#echo "test---------------------------------5"
	else

		if [ "$current_master" == "$masters" ]; then
			if [ "$kerb_ldap" == "yes" ]; then
			    install_kerbldap=1
				portmappings="$kerberos"
			else
				portmappings=
			fi
		else portmappings=
		fi

		#echo "test---------------------------------6"
		# Create a normal masternode container without port mappings
		docker run -i -t -d -v /sys/fs/cgroup:/sys/fs/cgroup:ro --tmpfs /run \
			-v ~/Downloads:/Downloads:ro \
			--name "$container_name" --hostname "$hostname" --privileged \
			-p 222$current_master:22 \
			$portmappings \
			--ip "$hostipaddr" \
			--network "$clustername" \
			-e "JAVA_HOME=/usr/java/latest" \
			masternode
	fi

	echo "Configuring Services on container ======> $container_name"

	#Common Actions on Container
	# update the java alternatives to the installed rpm
	
	docker exec -d "$container_name" bash -c 'localedef -i en_US -f UTF-8 en_US.UTF-8'
	docker exec -d "$container_name" systemctl enable httpd
	docker exec -d "$container_name" systemctl start httpd
	docker exec -d "$container_name" bash -c 'echo 1 | update-alternatives --config java'
	docker exec -t "$container_name" bash -c 'echo "
export JAVA_HOME=/usr/java/default
export PATH=\$JAVA_HOME/bin:\$PATH" >> /root/.bash_profile'
	# j_home=`docker exec namenode printenv JAVA_HOME`
	# docker exec -d namenode unzip -o -j /tmp/RPMS/jce_*.zip -d $j_home/jre/lib/security/
	# docker exec -t "$container_name" unzip -o -j /root/RPMS/jce_*.zip -d /usr/java/default/jre/lib/security/

	#enable syslog  i.e. var/log/messages /var/log/secure logging
	docker exec -d "$container_name" bash -c 'sed -i "/ModLoad imjournal/s/^/#/g" /etc/rsyslogd.conf'
	docker exec -d "$container_name" bash -c 'sed -i "/IMJournalStateFile imjournal\.state/s/^/#/g" /etc/rsyslogd.conf'
	docker exec -d "$container_name" bash -c 'sed -i "s/OmitLocalLogging on/OmitLocalLogging off/g" /etc/rsyslogd.conf'

	#SSH settings-disable strict host checking for sshpass to work
	docker exec -d "$container_name" bash -c 'sed -i "s/#PermitRootLogin/PermitRootLogin/g" /etc/ssh/sshd_config'
	docker exec -d "$container_name" bash -c 'sed -i "s/UseDNS/#UseDNS/g" /etc/ssh/sshd_config'
	docker exec -d "$container_name" bash -c 'echo "UseDNS no" >> /etc/ssh/sshd_config'
	docker exec -t "$container_name" bash -c 'ssh-keygen -t rsa -f ~/.ssh/id_rsa -N "" -q'
	docker exec -t "$container_name" bash -c 'echo "StrictHostKeyChecking no" >> ~/.ssh/config'
	#important to create cacert directory before transferring the cacert from ldapserver to client
	docker exec -t $container_name bash -c 'mkdir -p /etc/openldap/cacerts'
	echo "Configured following services on the container =====> httpd,java,jce,syslog,sshd"
	docker exec -t "$container_name" bash -c 'echo "waiting for system to be ready"'
	docker exec -d "$container_name" service sshd restart
	sleep 10

	#echo "test---------------------------------8"
	# Add hosts entries for all master and datanodes in /etc/hosts
	# echo "Adding masternode entries to Hosts file"
	#mnodes_start=1
	#mnodes_stop="$masters"
    echo "Adding all masternode entries to ======> /etc/hosts"
	for ((master = 1; master <= $masters; master++)); do
		#echo "adding -->$networkid$master $masternode_prefix$master.$domain_name"
		docker exec -t "$container_name" bash -c "echo '$networkid$master    $masternode_prefix$master.$domain_name    $masternode_prefix$master ' >> /etc/hosts"
		#mnodes_start=$(expr "$mnodes_start" + 1)
	done

    echo "Adding all datanode entries to ======> /etc/hosts"
	dnodes_start=$(expr "$masters" + 1)            #starting datanode ip if masters=3
	dnodes_number=1                                # starting datanode name e.g. datanode1 has ip 4,  datanode2 has ip 5 ...so on
	dnodes_stop=$(expr "$masters" + "$dnodes" ) # ending datanode ip , if number of dnodes =2 then dnodes range  = [4,5]

	#echo "test---------------------------------8"

	while [ "$dnodes_start" -le "$dnodes_stop" ] && [ "$dnodes" -gt "0" ]; do

		#echo "adding -->$networkid$dnodes_start $datanode_prefix$dnodes_number.$domain_name"
		docker exec -t "$container_name" bash -c "echo '$networkid$dnodes_start     $datanode_prefix$dnodes_number.$domain_name    $datanode_prefix$dnodes_number' >> /etc/hosts"
		dnodes_start=$((dnodes_start + 1))
		dnodes_number=$((dnodes_number + 1))
	done



	echo "Configuring network"
	#echo $hostname
	#Network Settings
	docker exec -t "$container_name" bash -c 'echo "
NETWORKING=yes
HOSTNAME $(hostname)" >> /etc/sysconfig/network'

	# updates the known_hosts file on kerbldap if datanode1 container was reinitialized
	# notice that we are executing ssh-copy-id on kerbldap to authorize connection from kerbldap
	# using " double quotes will evaluate shell varaible in current shell
	
	
	#Installing kerberos and Ldap if install_kerbldap=1
	if [ "$install_kerbldap" -eq 1 ]; then

     # ENV variable kerbldap_hostname
     # ENV variable kerbldap_container
		docker exec -t "$container_name" systemctl enable slapd
		docker exec -t "$container_name" systemctl start slapd
        . ./krb5.sh $domain_name $kerbldap_hostname $kerbldap_container
		. ./kerbldap.sh $container_name $domain_name $kerbldap_hostname
	fi
    # Installing mysql
	echo "Installing mysql"
	if [ "$current_master" == "1" ]; then
       
	   docker exec -t "$container_name" systemctl enable mysqld.service
	   docker exec -t "$container_name" systemctl start mysqld.service
	else
	   docker exec -t "$container_name" systemctl stop mysqld.service
	   docker exec -t "$container_name" systemctl disable mysqld.service
    fi

	echo "moving any online yum repos to /root/"
	# removing online repos.
	docker exec -d "$container_name" mkdir -p /root/yum.repos.d
	docker exec -d "$container_name" bash -c 'mv /etc/yum.repos.d/*  /root/yum.repos.d'


    echo "Container $container_name Initialization Successful"
	echo "---------------------------------------------------"
fi
