FROM nginx:1.17.3-alpine AS base
EXPOSE 8080
COPY nginx.conf /etc/nginx/nginx.conf
COPY src/ /usr/share/nginx/html