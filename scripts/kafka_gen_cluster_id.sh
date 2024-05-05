#!/bin/bash

docker run -it --rm \
    --name kafka-gen-cluster-id \
    --entrypoint "/bin/kafka-storage" \
    confluentinc/cp-kafka:7.5.0 \
    random-uuid
