Build:
```
docker compose -p boiler-plate -f docker-compose.yml build --no-cache
```
___
Deploy:
```
docker compose -p boiler-plate -f docker-compose.yml up -d --no-build
```
___
Undeploy:
```
docker compose -p boiler-plate down
```
___
Restart service:
```
docker compose -p boiler-plate restart <service_name>
```
