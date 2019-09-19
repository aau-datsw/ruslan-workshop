#!/bin/bash

# Create a directory for it
mkdir scripts/cert-gen/"$1"

# Generate the key
sudo openssl genrsa -out scripts/cert-gen/"$1"/privkey.pem 2048

# Generate the certificate
sudo openssl req -new -x509 -key scripts/cert-gen/"$1"/privkey.pem -out scripts/cert-gen/"$1"/fullchain.pem -days 3650 -subj /CN="$1"