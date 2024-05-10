# FROM node:20.8 as builder

# WORKDIR /app

# COPY frontend/. .
# RUN yarn
# RUN npm run build

FROM nginx:1.25.3 as server

# COPY --from=builder /app/build /app/html

CMD ["nginx", "-g", "daemon off;"]
