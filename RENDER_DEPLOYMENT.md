# Deploying DashyBoard to Render

## Quick Deploy (Blueprint)

1. Push this repo to GitHub
2. Go to [Render Dashboard](https://dashboard.render.com/)
3. Click **New** → **Blueprint**
4. Connect your GitHub repo
5. Render will detect `render.yaml` and create both services

## Manual Setup

### Backend API

1. **New Web Service** → Connect GitHub repo
2. Settings:
   - **Name**: `dashyboard-api`
   - **Root Directory**: (leave empty)
   - **Runtime**: Docker
   - **Dockerfile Path**: `./backend/Dockerfile`
   - **Docker Context**: `.`

3. Environment Variables:

   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ConnectionStrings__DefaultConnection=Data Source=/app/data/DashyBoard.db
   Jwt__Secret=<generate-a-secure-32+-char-string>
   ```

4. Optional API Keys (add as needed):
   ```
   Swedavia__FlightInfoApiKey=<your-key>
   Swedavia__WaitTimeApiKey=<your-key>
   CheckWx__ApiKey=<your-key>
   GitHub__ClientId=<your-client-id>
   GitHub__ClientSecret=<your-secret>
   ```

### Frontend

1. **New Static Site** → Connect GitHub repo
2. Settings:
   - **Name**: `dashyboard-frontend`
   - **Root Directory**: `frontend`
   - **Build Command**: `npm ci && npm run build`
   - **Publish Directory**: `dist`

3. Environment Variables:

   ```
   VITE_BASE_URL=https://dashyboard-api.onrender.com
   ```

   ⚠️ Replace with your actual backend URL after it deploys!

4. Add Rewrite Rule:
   - Source: `/*`
   - Destination: `/index.html`
   - Action: Rewrite

## Post-Deployment

### 1. Update Frontend API URL

After backend deploys, copy its URL (e.g., `https://dashyboard-api-xxxx.onrender.com`) and:

1. Go to frontend service → Environment
2. Update `VITE_BASE_URL` with actual backend URL
3. Trigger a redeploy

### 2. Seed Database (Optional)

Connect to backend shell and run:

```bash
sqlite3 /app/data/DashyBoard.db < /app/seed.sql
```

Or use the API's CSV import endpoint.

### 3. Add Health Check

Backend includes `/health` endpoint. Render auto-detects it.

## ⚠️ Important: SQLite Limitations

**Free tier uses ephemeral storage!** Your SQLite database will be:

- ✅ Created on first request
- ❌ **Deleted** when service restarts or redeploys

### Solutions for Persistent Data:

| Option               | Effort | Cost      |
| -------------------- | ------ | --------- |
| Render Disk          | None   | $7/mo     |
| Turso (SQLite cloud) | Low    | Free tier |
| Neon PostgreSQL      | Medium | Free tier |

### Switching to Turso (Recommended)

1. Create account at [turso.tech](https://turso.tech)
2. Create database: `turso db create dashyboard`
3. Get URL: `turso db show dashyboard --url`
4. Get token: `turso db tokens create dashyboard`
5. Update connection string in Render:
   ```
   ConnectionStrings__DefaultConnection=Data Source=libsql://dashyboard-xxx.turso.io;AuthToken=your-token
   ```

## Troubleshooting

### Backend won't start

- Check logs for connection string issues
- Ensure `ASPNETCORE_URLS` is set to `http://+:8080`

### Frontend shows API errors

- Verify `VITE_BASE_URL` matches actual backend URL
- Check browser console for CORS errors
- Backend CORS should allow frontend origin

### Database is empty after redeploy

- Expected on free tier (ephemeral storage)
- Re-import data via CSV or switch to persistent storage

## URLs After Deploy

- **Backend API**: `https://dashyboard-api.onrender.com`
- **Frontend**: `https://dashyboard-frontend.onrender.com`
- **API Docs**: `https://dashyboard-api.onrender.com/swagger`
