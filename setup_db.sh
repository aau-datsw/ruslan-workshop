docker-compose -f docker-compose.server.yml run market-place rails db:migrate
docker-compose -f docker-compose.server.yml run market-place rails db:setup