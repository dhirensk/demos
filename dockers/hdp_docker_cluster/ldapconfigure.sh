#!/bin/bash
#ENV variables kerbldap_container <-- masternode.sh
#ENV variables kerbldap_hostname <-- masternode.sh


array=("${!1}")
domain=$2
containers="${array[@]}"
echo "
<==================================== Configuring LDAP Authentication on all Nodes ======================================>"
for ((i = 0; i < "${#array[@]}"; i++)); do
    container_name="${array[i]}"
	#echo "other container $container_name"
	#echo "kerbldap_container $kerbldap_container"
	#echo "kerbldap_hostname $kerbldap_hostname"
	hostname=$(docker exec -t $container_name bash -c 'hostname' | tr -d '\r')
	#https://unix.stackexchange.com/questions/487117/variable-containing-output-of-docker-exec-command-malaligned
	# https://github.com/moby/moby/issues/8513 since linux terminal uses CR+LF, when using output on host clean up \r
	#echo "hostname $hostname"
    
	if [ "$kerbldap_container" != "$container_name" ]; then
        docker exec -t "$container_name" bash -c 'echo "TLS_CACERT /etc/openldap/cacerts/CA.crt" >> /etc/openldap/ldap.conf'
		docker exec -t "$kerbldap_container" bash -c "scp /etc/openldap/cacerts/CA.crt root@$hostname:/etc/openldap/cacerts/CA.crt"
		
		# escaping backslash using backslash to escape the double quotes expansion of \ when using double quotes
        docker exec -t "$container_name" bash -c "authconfig --enableldap --enableldapauth \\
		--ldapserver='$kerbldap_hostname' --ldapbasedn='dc=$domain,dc=com' --enablesssd \\
		--enablesssdauth --enableldaptls --enablemkhomedir --disablecache --disablecachecreds --update"
        echo "LDAP authentication successfully configured on $container_name"
	fi
done



