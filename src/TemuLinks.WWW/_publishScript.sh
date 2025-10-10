

rsync --recursive --checksum --verbose wwwroot/* root@ebcsrv11.ebc.rocks:/var/www/temulinks/WWW

ssh root@ebcsrv11.ebc.rocks 'systemctl restart nginx.service'
