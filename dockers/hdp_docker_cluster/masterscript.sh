#!/bin/bash
exitloop=false
while ! $exitloop; do

	echo "
>>>>>> Docker Based HDP Cluster <<<<<<

"
	read -p "Enter number from below Actions
1. Install Docker Images
2. Container/Cluster Initialization
3. Show all Images and Containers
4. Start Cluster
5. Stop Cluster
6. Delete Cluster
7. Install Hortonworks Data Platform
0. Exit

Enter your choice : " action
	echo""
	case "$action" in
	1)
		read -p "Proceed with Docker Images and Container Initialization? yes/no : " answer
		if [ "$answer" != "yes" ]; then exit; fi
		# 1. Install Docker Images and Initialize Containers
		echo "
>>>>>> Searching for Dockerfiles <<<<<<
"
		i=0
		for file in *.Dockerfile; do
			i=$((i + 1))
		done

		echo "found $i Dockerfiles"

		for file in *.Dockerfile; do
			echo -e "$file "

		done

		echo "
>>>>>> Processing Dockerfiles <<<<<<
"
		for file in *.Dockerfile; do
			#echo Building$file
			filename=$(sed -e "s/\(.*\)\.Dockerfile/\1/g" <<<"$file")
			# echo $filename
			docker images | grep -q -w "$filename"
			if [ $? -eq 0 ]; then
				echo "Docker Image : $filename already exists"
			else
				echo "Building Docker image using $file"
				docker build -t $filename -f $file .  2>&1 | tee "$filename"_install.log
			fi
		done
		echo "Below docker images exist in the system"
		docker images

		;;
	2)
		read -p "Docker container/cluster initialization?  yes/no : " cont
		if [ "$cont" != "yes" ]; then exit; fi

		echo "
>>>>>> Cluster Installation <<<<<<
"
		while true; do
			read -p "Enter the name of your cluster : " clustername
			docker ps -a | grep -q -w "$clustername"
			if [ $? -eq 0 ]; then
				echo "
cluster $clustername already exists. Enter a different cluster name
"
			else
				break
			fi
		done

		echo " "
		while true; do
			read -p "Enter Number of master nodes: " masters
			if [ "$masters" -lt 1 ] || [ "$masters" == "" ]; then
				echo "master nodes cannot be less than 1 or not a number"
				exit
			else
				break
			fi
		done

		echo ""
		while true; do
			read -p "Enter number of datanodes to be setup.
Type 0 or press enter to skip datanodes : " dnodes
			if [ "$dnodes" -lt 0 ]; then
				echo " data nodes cannot be less than 0 "
			elif [ "$dnodes" == "" ]; then
				dnodes=0
				break
			else
				break
			fi
		done
		echo " "
		read -p "Enter Domain name e.g. mydocker.com 
Press enter to use a default domain (dkdocker.com) : " domain_name
		if [ "$domain_name" == "" ]; then
			domain_name="dkdocker.com"
		fi
		domain=$(echo $domain_name | sed 's/\(.*\)\..*/\1/')
		echo ""

		while true; do
			read -p "Provide network subnet to use for this cluster 
Press enter to use a default subnet 172.16.0.0/24.
Please do not use 172.17.0.0/24 as it is assigned to default bridge: " subnet
			if [ "$subnet" == "" ] ; then
				subnet="172.16.0.0/24"
			fi

			#returns a list of names of all docker networks
			#We create our own subnet so as to assign our own set of fixed static IPs to the container
            networkid=$(echo $subnet | sed 's/\([0-9]*\.\)\([0-9]*\.\)\([0-9]*\.\).*/\1\2\3/')
			for nw in $(docker network ls --format "{{.Name}}"); do
				docker network inspect "$nw" | grep -q "$networkid"
				if [ $? -eq 0 ]; then
					echo "subnet $subnet already used in network $nw. Try different subnet"
					subnet=""				
				fi
			done
			if [ "$subnet" != "" ]; then
				docker network ls | grep -q "$clustername"
				if [ $? -eq 1 ]; then
					docker network create --gateway "$networkid"24 --subnet "$subnet" \
						--opt com.docker.network.bridge.name="$clustername"0 \
						--driver bridge \
						$clustername
				fi
				break
			fi
		done

		echo ""
		read -p "Enter the masternode hostname prefix  
e.g. using masternode as prefix will create masternode1, masternode2 ... 
press enter for default (masternode): " masternode_prefix
		if [ "$masternode_prefix" == "" ]; then
			masternode_prefix="masternode"
		fi
		echo ""
		read -p "Enter the datanode hostname prefix 
e.g  using datanode as prefix will create datanode1, datanode2 ...
press enter for default (datanode) : " datanode_prefix
		if [ "$datanode_prefix" == "" ]; then
			datanode_prefix="datanode"
		fi
		echo ""

		read -p "Install kerberos and ldap yes/no : " kerb_ldap
		if [ "$kerb_ldap" == "yes" ]; then
			echo "Kerberos and ldap will be installed on last masternode"
		fi
		totalnodes=$(expr $masters + $dnodes)
		echo "
		
<====== CONFIGURATION SUMMARY ======>
"
		echo "Total number of Master Nodes         : $masters"
		echo "Total number of Data Nodes           : $dnodes"
		echo "Total number of Nodes in the cluster : $totalnodes"
		echo "Domain Name                          : $domain_name"
		echo "Cluster Name                         : $clustername"
		echo "Masternodes Prefix                   : $masternode_prefix"
		echo "Datanodes Prefix                     : $datanode_prefix"
		echo "Install Kerberos and LDAP Services   : $kerb_ldap"
		echo "Network Subnet                       : $subnet"
		echo ""
		read -p "Continue yes/no ?" yesorno
		case "$yesorno" in
		"yes") 
		     ;;

		
		"no")
			exit
			;;
		*)
			exit
			;;
		esac
		totalnodes=$(expr $masters + $dnodes)

		domain=$(echo $domain_name | sed 's/\(.*\)\..*/\1/') #return first part of domain name

        if [ "$kerb_ldap" == "yes" ]; then
		# exports 2 environmental variables to identify kerbldap_container and kerbldap_hostname if kerb_ldap=yes
		# It is the last masternode container
		    
		   export kerbldap_container="$clustername"_"$masternode_prefix""$masters"
           export kerbldap_hostname="$masternode_prefix""$masters"."$domain_name"
        fi

		echo ""
		echo "<====== Initializing Masternodes ======>"
		i=1
		while [ "$i" -le "$masters" ]; do
			. ./masternode.sh $i $masters $dnodes $totalnodes $domain_name $clustername $masternode_prefix $datanode_prefix $kerb_ldap $subnet
			i=$((i + 1))
		done
		
        
		i=1
		echo "<====== Initializing Datanodes ======>"
		while [ "$i" -le "$dnodes" ] && [ "$dnodes" -ne 0 ]; do
			#echo "calling sh  datanode.sh $i"
			. ./datanode.sh $i $masters $dnodes $totalnodes $domain_name $clustername $masternode_prefix $datanode_prefix $kerb_ldap $subnet
			i=$((i + 1))
		done

		#looping over all nodes, get a list of all containers
		i=1
		declare -a containers
		while [ "$i" -le "$masters" ]; do
			container_name="$clustername"_"$masternode_prefix""$i"
			#containers[$i]+=($container_name)
			containers+=($container_name)
			i=$((i + 1))
		done
		i=1
		while [ "$i" -le "$dnodes" ]; do
			container_name="$clustername"_"$datanode_prefix""$i"
			containers+=($container_name)
			i=$((i + 1))
		done

		# echo "even before containers ${containers[@]}"
		# Configuring ssh on all the containers
		#. ./sshconfigure.sh "${containers[@]}"  # pass by value  
		
		. ./sshconfigure.sh "containers[@]"  # Pass by reference
		#Configuring the krb5.conf on all nodes
        # echo "before containers ${containers[@]}"
		if [ "$kerb_ldap" == "yes" ]; then
		    
			echo "Configuring /etc/krb5.conf on rest of the nodes"
			#echo "${containers[@]}"
			#echo "${#containers[@]}"
			for container in "${containers[@]}" ; do
			#echo $container
			otherhostname=$(docker exec -t $container bash -c 'hostname' | tr -d '\r')
			#echo "otherhostname $otherhostname"
              if [ "$kerbldap_container" != "$container" ] ; then
               docker exec -t $kerbldap_container bash -c "scp /etc/krb5.conf root@$otherhostname:/etc/"  #using " instead of '
			     #TODO
			  fi
			done


		fi



		# Configuring ldap client on all the containers
		if [ "$kerb_ldap" == "yes" ]; then
			. ./ldapconfigure.sh "containers[@]" $domain
		fi

		docker ps -a --format 'table {{.Names}}\t{{.Image}}\t{{.RunningFor}}\t{{.Status}}'
		echo "



-------------------------------------------------------------------------------
***************************IMPORTANT INFORMATION*******************************
-------------------------------------------------------------------------------

Common Servies & Utilities                      :ssh, telnet, jdk 1.8, ntp, systemd,
                                                 rsyslog, httpd, ssl, kerberos client, sssd,
                                                 authconfig, network,wget, IST Timezone
${clustername}_${masternode_prefix}${masters}   :kerberos server, kadmin server, openldap server
${clustername}_${masternode_prefix}1            :mysql server, ambari server

local user root password : root exists on all hosts
ldap user dhiren password : dhiren exist on ldap

to connect to any of the containers : 
ssh root@localhost -p ####
Container Port range from 2221:222$totalnodes

kerberos Admin principal : admin/admin@${domain_name^^}
kadmin password : admin

openldap admin DN : cn=admin,dc=${domain},dc=com
admin password    : admin

*******************************************************************************
"

		;;

	3)

		docker images
		echo " "
		 docker ps -a --format 'table {{.Names}}\t{{.Image}}\t{{.RunningFor}}\t{{.Status}}'
		;;

	4)
		read -p "Start Cluster? yes/no
: " answer
		if [ "$answer" == "yes" ]; then
			echo "Found below clusters : "
			echo "$(docker ps -a | grep -o -w "[a-zA-Z0-9]*_.*" | sed 's/\(.*\)_.*/\1/' | sort | uniq)"
			read -p "Enter the cluster name :" c_name
			docker start $(docker ps -a | grep -o -e "$c_name"_.*)
            echo "Containers started Successfully"
		fi
		;;

	
	5)
		read -p "Stop Cluster? yes/no
: " answer
		if [ "$answer" == "yes" ]; then
			echo "Found below running clusters : "
			echo "$(docker ps | grep -o -w "[a-zA-Z0-9]*_.*" | sed 's/\(.*\)_.*/\1/' | sort | uniq)"
			read -p "Enter the cluster name :" c_name
			docker stop $(docker ps -a | grep -o -e "$c_name"_.*)
            echo "Containers stopped Successfully"
		fi
		;;
	6)
		read -p "Delete Cluster? provide clustername
: " del_cluster
		if [ "$del_cluster" != "" ]; then
			#echo "TODO: Make sure to delete the subnet associated with container"
			docker ps -a | grep -q "$del_cluster""_.*"
			if [ $? -eq 0 ]; then
				echo ""
				echo "stopping containers...
			"
				docker stop $(docker ps -a | grep -o -w -e "$del_cluster"_.*)
				echo ""
				echo "deleting containers...
			"
				docker rm $(docker ps -a | grep -o -w -e "$del_cluster"_.*)
				docker container prune -f
				echo ""
				echo -n "deleting network..."
				docker network rm $del_cluster
				echo ""
				echo "Cleaning up known_hosts"
				>~/.ssh/known_hosts
			else
				echo "Cluster $del_cluster does not exist"
			fi
		else
			echo "Provide Cluster name "
		fi
		;;

	7)
		read -p "Install HDP Ambari Server ? yes/no
: " answer
		if [ "$answer" == "yes" ]; then
			echo "Make sure host has below files. Host's  ~/Downloads is mounted as /Downloads on each container
1. ~/Downloads/HDP-UTILS-<version>-centos7.tar
2. ~/Downloads/HDP-<version>-centos7-rpm.tar
3. ~/Downloads/ambari-<version>-centos7.tar
4. Update mysqlhdp.sh file accordingly as per the package versions
"
			read -p "Continue? yes/no
  : " value
			if [ "$value" == "yes" ]; then
			docker ps -a --format 'table {{.Names}}\t{{.Image}}\t{{.RunningFor}}\t{{.Status}}'
			read -p "Provide running container name from above on which Ambari is to be installed : " namenode
			. ./mysqlhdp.sh $namenode
			fi
		fi
		;;

	0)
		exitloop=true
		;;
	*)
		echo "Enter Correct Choice"

		;;

	esac

done
