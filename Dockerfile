FROM caddy:2-alpine

COPY Caddyfile /etc/caddy/Caddyfile
COPY ./game/build/Latest /usr/share/caddy
