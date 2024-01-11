#!/bin/bash
current_dnode=$1
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

# echo "current_dnode $current_dnode"
# echo "masters $masters"
# echo "dnodes $dnodes"
# echo "totalnodes $totalnodes"
# echo "domain_name $domain_name"
# echo "clustername $clustername"
# echo "masternode_prefix $masternode_prefix"
# echo "datanode_prefix $datanode_prefix"
# echo "kerb_ldap $kerb_ldap"
# echo "subnet $subnet"
echo ""
echo "Initializing container for ======> $datanode_prefix$current_dnode"

#container name format  HadoopDev_datanode1
container_name="$clustername"_"$datanode_prefix""$current_dnode"

#hostname format  datanode1.dkdocker.com
hostname="$datanode_prefix""$current_dnode"."$domain_name"
# returns 172.16.0. if the subnet is 172.16.0.0/24
networkid=$(echo $subnet | sed 's/\([0-9]*\.\)\([0-9]*\.\)\([0-9]*\.\).*/\1\2\3/')
hostip=$(expr $masters + "$current_dnode")
hostipaddr="$networkid""$hostip"
docker ps -a | grep -q -w "$container_name"
if [ $? -eq 0 ]; then
	docker start "$container_name"
else
	docker run -i -t -d -v /sys/fs/cgroup:/sys/fs/cgroup:ro --tmpfs /run \
		-v ~/Downloads:/Downloads \
		--name "$container_name" --hostname "$hostname" --privileged \
		-p 222$hostip:22 \
		--ip "$hostipaddr" \
		--network "$clustername" \
		-e "JAVA_HOME=/usr/java/latest" \
		datanode

	echo "Configuring services on container ======> $container_name"
	docker exec -d "$container_name" bash -c 'localedef -i en_US -f UTF-8 en_US.UTF-8'
	# update the java alternatives to the installed rpm
	docker exec -d "$container_name" bash -c 'echo 1 | update-alternatives --config java'
	docker exec -d "$container_name" bash -c 'echo "
export JAVA_HOME=/usr/java/default
export PATH=\$JAVA_HOME/bin:\$PATH" >> /root/.bash_profile'
	docker exec -d "$container_name" unzip -o -j /root/RPMS/jce_*.zip -d /usr/java/default/jre/lib/security/
	#enable syslog  i.e. var/log/messages /var/log/secure logging
	docker exec -d "$container_name" bash -c 'sed -i "/ModLoad imjournal/s/^/#/g" /etc/rsyslogd.conf'
	docker exec -d "$container_name" bash -c 'sed -i "/IMJournalStateFile imjournal\.state/s/^/#/g" /etc/rsyslogd.conf'
	docker exec -d "$container_name" bash -c 'sed -i "s/OmitLocalLogging on/OmitLocalLogging off/g" /etc/rsyslogd.conf'
	docker exec -d "$container_name" bash -c 'sed -i "s/#PermitRootLogin/PermitRootLogin/g" /etc/ssh/sshd_config'
	docker exec -d "$container_name" bash -c 'sed -i "s/UseDNS/#UseDNS/g" /etc/ssh/sshd_config'
	docker exec -d "$container_name" bash -c 'echo "UseDNS no" >> /etc/ssh/sshd_config'
	docker exec -t "$container_name" bash -c 'ssh-keygen -t rsa -f ~/.ssh/id_rsa -N "" -q'
	docker exec -t "$container_name" bash -c 'echo "StrictHostKeyChecking no" >> ~/.ssh/config'
	docker exec -t "$container_name" bash -c 'mkdir -p /etc/openldap/cacerts'
	docker exec -d "$container_name" service sshd restart
	echo "Configured following services on the container =====> httpd,java,jce,syslog,sshd"
	docker exec -t "$container_name" bash -c 'echo "waiting for system ready up"'
	sleep 10
	# updates the known_hosts file on kerbldap if datanode1 container was reinitialized
	# notice that we are executing ssh-copy-id on kerbldap to authorize connection from kerbldap
	# using " double quotes will evaluate shell varaible in current shell

echo "Adding all masternode entries to ======> /etc/hosts"
	for ((master = 1; master <= $masters; master++)); do
			#echo "adding -->$networkid$master $masternode_prefix$master.$domain_name"
			docker exec -t "$container_name" bash -c "echo '$networkid$master    $masternode_prefix$master.$domain_name    $masternode_prefix$master ' >> /etc/hosts"
	done

	dnodes_start=$(expr "$masters" + 1)        #starting datanode ip if masters=3
	dnodes_number=1                            # starting datanode name e.g. datanode1 has ip 4,  datanode2 has ip 5 ...so on
	dnodes_stop=$(expr "$masters" + "$dnodes") # ending datanode ip , if number of dnodes =2 then dnodes range  = [4,5]

	

echo "Adding all datanode entries to ======> /etc/hosts"
	while [ "$dnodes_start" -le "$dnodes_stop" ]; do
		if [ "$dnodes_start" != "$current_dnode" ]; then
		#echo "adding -->$networkid$dnodes_start $datanode_prefix$dnodes_number.$domain_name"
		docker exec -t "$container_name" bash -c "echo '$networkid$dnodes_start     $datanode_prefix$dnodes_number.$domain_name    $datanode_prefix$dnodes_number' >> /etc/hosts"
    fi
		dnodes_start=$((dnodes_start + 1))
		dnodes_number=$((dnodes_number + 1))
	done



	echo "Configuring network"
	docker exec -t "$container_name" bash -c 'echo "
NETWORKING=yes
HOSTNAME $(hostname)" >> /etc/sysconfig/network'

   echo "moving any online yum repos to /root/ "
	# removing online repos.
	docker exec -d "$container_name" mkdir -p /root/yum.repos.d
	docker exec -d "$container_name" bash -c 'mv /etc/yum.repos.d/*  /root/yum.repos.d'

    echo "Container $container_name Initialization Successful"
	echo "---------------------------------------------------"

fi

