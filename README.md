# ExchangeRate.Aggregator

This repo is used as a reference for Modular Monolithic Architecture: 
https://github.com/devmentors/Inflow/tree/master/src

Future improvements:
1. Fluent validation
2. Add postgreSQL connection to docker-compose (the container creation is finished but it can't connect to database)
3. Lookup APIs
4. Separate DB for parser and data synchronization.
5. Unit tests


Run the following projects:
1. ExchangeRate.Aggregator.Bootstrapper
2. ExchangeRate.Aggregator.Modules.Parsers

Dockerfiles and docker compose are also available.
