#!/bin/bash

DIRNAME=$(cd "$(dirname "${0}")" && pwd)
PROJECT_ROOT=$(cd "${DIRNAME}/.." && pwd)

OPTIONS="t:"
LONGOPTS="topic:"

! PARSED=$(getopt "--options=${OPTIONS}" "--longoptions=${LONGOPTS}" --name "${0}" -- "$@")
if [[ ${PIPESTATUS[0]} -ne 0 ]]; then
  exit 2
fi

eval set -- "${PARSED}"

TOPIC_NAME="0"
ACTION="describe"

while true; do
  case "${1}" in
    -t|--topic)
      TOPIC_NAME="${2}"
      shift 2
      ;;
    --)
      shift
      break
      ;;
    *)
      echo "Error: Unexpected option: ${1}"
      exit 3
      ;;
  esac
done

if [[ "${TOPIC_NAME}" == "0" ]]; then
  echo "Error: Please add -t/--topic with your topic name"
  exit 2
fi

docker exec -it kafka kafka-console-consumer.sh --topic=${TOPIC_NAME} --from-beginning --bootstrap-server :9092
