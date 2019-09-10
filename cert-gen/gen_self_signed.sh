#!/bin/bash

# Create a directory for it
sudo mkdir -p /etc/letsencrypt/live/"$1"/
sudo cd /etc/letsencrypt/live/"$1"/
 
# Generate the key
sudo openssl genrsa -out privkey.pem 2048
# Generate the certificate
sudo openssl req -new -x509 -key privkey.pem -out fullchain.pem -days 3650 -subj /CN="$1"