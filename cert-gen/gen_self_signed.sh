#!/bin/bash

# Create a directory for it
mkdir cert-gen/"$1"

# Generate the key
sudo openssl genrsa -out cert-gen/"$1"/privkey.pem 2048

# Generate the certificate
sudo openssl req -new -x509 -key cert-gen/"$1"/privkey.pem -out cert-gen/"$1"/fullchain.pem -days 3650 -subj /CN="$1"