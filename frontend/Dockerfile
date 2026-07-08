# ---- Build faza ----
FROM node:22-slim AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
# Production build (SSR: proizvodi dist/my-app/{browser,server})
RUN npm run build

# ---- Runtime faza ----
FROM node:22-slim AS runtime
WORKDIR /app
ENV NODE_ENV=production
ENV PORT=4200

# Samo runtime zavisnosti (express, @angular/ssr, ...)
COPY package*.json ./
RUN npm ci --omit=dev

COPY --from=build /app/dist ./dist

EXPOSE 4200
CMD ["node", "dist/my-app/server/server.mjs"]
