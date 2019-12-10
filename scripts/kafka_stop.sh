#!/bin/bash

DIRNAME=$(cd "$(dirname "${0}")" && pwd)
PROJECT_ROOT=$(cd "${DIRNAME}/.." && pwd)

cd "${PROJECT_ROOT}"

docker-compose down
