Kafka & zookeeper are made with a compose file

setting up..

### creating the topic ( here for address-events )
```sh
docker-compose exec kafka kafka-topics --create --topic address-events --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1
```

### listing the topic
```sh
docker-compose exec kafka kafka-topics --list --bootstrap-server localhost:9092
```