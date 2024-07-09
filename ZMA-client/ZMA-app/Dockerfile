FROM node:20.11.0
WORKDIR /app
ENV API_BACKEND_URL=${API_BACKEND_URL}
COPY package.json package-lock.json ./
RUN npm install
COPY . .
EXPOSE 3000
CMD [ "npm", "start" ]