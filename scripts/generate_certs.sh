# Create a self-signed certificate for ruslan.local so we can develop locally
sudo sh cert-gen/gen_self_signed.sh ruslan.local
sudo sh cert-gen/gen_self_signed.sh api.ruslan.local 

# Make letsencrypt directories in /etc/ 
sudo mkdir -p /etc/letsencrypt/live/ruslan.local 
sudo mkdir -p /etc/letsencrypt/live/api.ruslan.local

# Put the certs there
sudo mv cert-gen/ruslan.local/privkey.pem /etc/letsencrypt/live/ruslan.local/
sudo mv cert-gen/ruslan.local/fullchain.pem /etc/letsencrypt/live/ruslan.local/
sudo mv cert-gen/api.ruslan.local/privkey.pem /etc/letsencrypt/live/api.ruslan.local/
sudo mv cert-gen/api.ruslan.local/fullchain.pem /etc/letsencrypt/live/api.ruslan.local/

