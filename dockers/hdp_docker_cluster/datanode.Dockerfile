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
# update locatetime to IST
RUN rm /etc/localtime
RUN ln -s /usr/share/zoneinfo/Asia/Kolkata /etc/localtime
RUN yum clean all
#RUN yum -y update
RUN yum -y install epel-release zip unzip openssh-server openssh-clients sshpass  ntp ntpdate ntp-doc
RUN yum -y install net-tools bind-utils telnet telnet-server httpd initscripts wget openssl man man-pages rsyslog
RUN yum -y install sssd sssd-client redhat-lsb python-devel gcc rpcbind libtirpc-devel nc
RUN yum -y install krb5-libs krb5-workstation authconfig-gtk oddjob-mkhomedir postgresql postgresql-libs postgresql-server
RUN mkdir -p /usr/share/java
RUN mkdir /root/RPMS
COPY RPMS/* /root/RPMS/
RUN mkdir -p /usr/share/java
COPY MYSQL/mysql-connector-java-*.jar /usr/share/java/mysql-connector-java.jar
RUN rpm -i /root/RPMS/*.rpm
ENV JAVA_HOME /usr/java/latest

RUN mkdir /var/run/sshd
RUN echo 'root:root' | chpasswd
RUN sed -i 's/#PermitRootLogin without-password/PermitRootLogin yes/' /etc/ssh/sshd_config

# SSH login fix. Otherwise user is kicked off after login
RUN sed 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' -i /etc/pam.d/sshd

ENV NOTVISIBLE "in users profile"
RUN echo "export VISIBLE=now" >> /etc/profile
RUN systemctl enable ntpd.service
RUN systemctl enable sssd.service
RUN systemctl enable oddjobd.service
RUN systemctl enable rsyslog.service

CMD ["/usr/sbin/init"]
