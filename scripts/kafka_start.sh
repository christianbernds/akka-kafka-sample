#!/bin/bash

DIRNAME=$(cd "$(dirname "${0}")" && pwd)
PROJECT_ROOT=$(cd "${DIRNAME}/.." && pwd)

cd "${PROJECT_ROOT}"

IPADDRESS=$(ipconfig getifaddr en0)

echo "Your local ip address is ${IPADDRESS}"

export LOCAL_IP_ADDRESS=${IPADDRESS}

docker-compose up -d
