#!/bin/bash

set -e # Exit on error

private_key=$(openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:2048)
public_key=$(echo "$private_key" | openssl rsa -pubout)
echo $private_key
echo $public_key