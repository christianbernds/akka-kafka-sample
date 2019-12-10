#!/bin/bash

DIRNAME=$(cd "$(dirname "${0}")" && pwd)
PROJECT_ROOT=$(cd "${DIRNAME}/.." && pwd)

OPTIONS="t:,a:"
LONGOPTS="topic:,action:"

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
    -a|--action)
      ACTION="${2}"
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

if [[ "${ACTION}" == "describe" ]]; then
  docker exec -it kafka kafka-topics.sh --describe --topic ${TOPIC_NAME} --bootstrap-server :9092
fi

if [[ "${ACTION}" == "create" ]]; then
  docker exec -it kafka kafka-topics.sh --create --topic ${TOPIC_NAME} --partitions 1 --replication-factor 1 --bootstrap-server :9092
fi

if [[ "${ACTION}" == "delete" ]]; then
  docker exec -it kafka kafka-topics.sh --delete --topic ${TOPIC_NAME} --bootstrap-server :9092
fi

