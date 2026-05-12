import { useState } from "react";
import {
  Box,
  Button,
  CircularProgress,
  Paper,
  TextField,
  Typography,
  Alert,
} from "@mui/material";
import { useNavigate } from "react-router";
import { login } from "../../services/api/login";
import { theme } from "../../theme";

export default function LoginPage() {
  const navigate = useNavigate();
  const [hotelId, setHotelId] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await login({ hotelId, password });
      localStorage.setItem("auth_token", response.token);
      navigate("/admin");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh",
        backgroundColor: theme.palette.background.default,
      }}
    >
      <Paper elevation={3} sx={{ p: 4, width: "100%", maxWidth: 400 }}>
        <Typography variant="h4" mb={3} textAlign="center">
          Admin Login
        </Typography>

        <Box component="form" onSubmit={handleSubmit} display="flex" flexDirection="column" gap={2}>
          <TextField
            label="Hotel ID"
            value={hotelId}
            onChange={(e) => setHotelId(e.target.value)}
            required
            fullWidth
            autoFocus
          />

          <TextField
            label="Lösenord"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            fullWidth
          />

          {error && <Alert severity="error">{error}</Alert>}

          <Button
            type="submit"
            variant="contained"
            fullWidth
            disabled={loading}
            sx={{ mt: 1 }}
          >
            {loading ? <CircularProgress size={24} /> : "Logga in"}
          </Button>
        </Box>
      </Paper>
    </Box>
  );
}
