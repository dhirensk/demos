#!/bin/bash

array=("${!1}")
Containers="${array[@]}"

echo "

<====================================== Configuring ssh passwordless for all nodes ======================================>"
for Container in $Containers; do
	#echo "container $container"
	for ((i = 0; i < "${#array[@]}"; i++)); do
		#if [ "$Container" != "${array[i]}" ]; then
			othercontainer="${array[i]}"
			#echo "other container $othercontainer"
			otherhostname=$(docker exec -t $othercontainer bash -c 'hostname' | tr -d '\r')
			#https://unix.stackexchange.com/questions/487117/variable-containing-output-of-docker-exec-command-malaligned
			# https://github.com/moby/moby/issues/8513 since linux terminal uses CR+LF, when using output on host clean up \r
            #echo "otherhostname $otherhostname"
			docker exec -t $Container bash -c "sshpass -p root ssh-copy-id -f -i ~/.ssh/id_rsa.pub root@$otherhostname"
		#fi
	done
done

echo "ssh passwordless configured Successfully on all nodes"

#Removes all keys belonging to hostname from a known_hosts file.  This option is useful to delete hashed hosts
#TODO docker exec -t kerbldap bash -c 'ssh-keygen -R 172.16.0.3'
#TODO docker exec -t kerbldap bash -c 'sshpass -p "root" ssh-copy-id -f -i ~/.ssh/id_rsa.pub root@namenode'
#TODO docker exec -t "$container_name" bash -c 'ssh-keygen -R 172.16.0.2'
#TODO docker exec -t "$container_name" bash -c 'sshpass -p "root" ssh-copy-id -f -i ~/.ssh/id_rsa.pub root@kerbldap'
#TODO docker exec -t "$container_name" bash -c 'sshpass -p "root" ssh-copy-id -f -i ~/.ssh/id_rsa.pub root@namenode'
