#https://docs.docker.com/samples/library/centos/
FROM centos/systemd
ENV container docker 
COPY CentOS-vault.repo /etc/yum.repos.d/
# update locatetime to IST
RUN rm /etc/localtime
RUN ln -s /usr/share/zoneinfo/Asia/Kolkata /etc/localtime
# RUN yum clean all
#RUN yum -y update
RUN yum -y downgrade openldap-2.4.44-5.el7.x86_64 
RUN yum -y install openldap-servers-2.4.44-5.el7.x86_64 openldap-clients-2.4.44-5.el7.x86_64
RUN yum -y install epel-release zip unzip openssh-server openssh-clients sshpass ntp ntpdate ntp-doc
RUN yum -y install net-tools bind-utils telnet telnet-server httpd initscripts wget openssl man man-pages rsyslog
RUN yum -y install sssd sssd-client authconfig-gtk oddjob-mkhomedir
RUN yum -y install redhat-lsb python-devel gcc rpcbind libtirpc-devel nc postgresql postgresql-libs postgresql-server
RUN yum -y install krb5-server krb5-libs krb5-workstation 
RUN wget https://dev.mysql.com/get/mysql80-community-release-el7-11.noarch.rpm
RUN yum -y localinstall mysql80-community-release-el7-11.noarch.rpm
RUN yum -y install mysql-community-server
RUN wget https://dev.mysql.com/get/Downloads/Connector-J/mysql-connector-j-8.2.0.zip
RUN mkdir -p /usr/share/java
RUN unzip -p mysql-connector-j-8.2.0.zip mysql-connector-j-8.2.0/mysql-connector-j-8.2.0.jar > /usr/share/java/mysql-connector-java.jar
# COPY MYSQL/my.cnf /etc/
# COPY MYSQL/mysql-connector-java-*.jar /usr/share/java/mysql-connector-java.jar
RUN yum install -y mysql-server
RUN chown -R ldap.ldap /var/lib/ldap
RUN mkdir /root/RPMS
COPY RPMS/*.rpm /root/RPMS/
RUN rpm -i /root/RPMS/*.rpm
ENV JAVA_HOME /usr/java/latest
 
RUN mkdir /var/run/sshd
RUN echo 'root:root' | chpasswd
RUN sed -i 's/#PermitRootLogin without-password/PermitRootLogin yes/' /etc/ssh/sshd_config

# SSH login fix. Otherwise user is kicked off after login
RUN sed 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' -i /etc/pam.d/sshd

ENV NOTVISIBLE "in users profile"
RUN echo "export VISIBLE=now" >> /etc/profile
RUN systemctl enable sssd.service
RUN systemctl enable ntpd.service
RUN systemctl enable oddjobd.service
RUN systemctl enable rsyslog.service

CMD ["/usr/sbin/init"]
