
FROM nginx:latest
## Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*
COPY --chown=nginx:nginx ./dist/ruslan-app /usr/share/nginx/html 
## From 'builder' stage copy over the artifacts in dist folder to default nginx public folder
CMD ["nginx", "-g", "daemon off;"]
