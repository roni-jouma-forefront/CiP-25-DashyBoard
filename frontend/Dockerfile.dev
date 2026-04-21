# Development image
FROM node:20-alpine
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Expose Vite dev server
EXPOSE 3000

# Start Vite in dev mode and listen on all interfaces
CMD ["npm", "run", "dev", "--", "--host"]
