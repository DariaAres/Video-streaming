FROM node:16 as node
ENV GENERATE_SOURCEMAP=false
ENV NODE_OPTIONS=--max-old-space-size=16384
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

FROM nginx:alpine
COPY nginx.default.conf /etc/nginx/conf.d/default.conf
COPY --from=node /app/dist/video-streaming /usr/share/nginx/html