ssh root@ebcsrv11.ebc.rocks 'systemctl stop api.temulinks.service'
rsync --recursive --checksum --verbose * root@ebcsrv11.ebc.rocks:/var/www/temulinks/API
ssh root@ebcsrv11.ebc.rocks 'systemctl start api.temulinks.service'

