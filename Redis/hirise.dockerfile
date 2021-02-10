FROM redis
COPY hirise-redis.conf /usr/local/etc/redis/hirise-redis.conf
CMD [ "redis-server", "/usr/local/etc/redis/hirise-redis.conf", "--appendonly", "yes" ]
