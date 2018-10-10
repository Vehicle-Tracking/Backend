FROM nginx

VOLUME /etc/nginx

COPY ./nginx /etc/nginx/

 CMD ["nginx", "-g", "daemon off;"]