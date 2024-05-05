# FROM node:20.8 as builder

# WORKDIR /app

# COPY frontend/. .
# RUN yarn
# RUN npm run build

FROM nginx:1.25.3 as server

ARG BOILER_PLATE_ENVIRONMENT

# COPY --from=builder /app/build /app/html
COPY configs/nginx/app /app
COPY configs/nginx/conf/nginx.${BOILER_PLATE_ENVIRONMENT}.conf /etc/nginx/conf.d/default.conf
COPY configs/nginx/certs /etc/certs

CMD ["nginx", "-g", "daemon off;"]
