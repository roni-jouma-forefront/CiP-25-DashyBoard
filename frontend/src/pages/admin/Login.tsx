import { useState } from "react";
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  IconButton,
  InputAdornment,
  TextField,
  Typography,
} from "@mui/material";
import { BadgeOutlined, HotelOutlined, LockOutlined, Visibility, VisibilityOff } from "@mui/icons-material";
import { useNavigate } from "react-router";
import { login } from "../../services/api/login";

export default function LoginPage() {
  const navigate = useNavigate();
  const [hotelId, setHotelId] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
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
      setError(err instanceof Error ? err.message : "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  const fieldSx = {
    "& .MuiOutlinedInput-root": {
      color: "#F1F5F9",
      "& fieldset": { borderColor: "rgba(255,255,255,0.15)" },
      "&:hover fieldset": { borderColor: "rgba(255,255,255,0.25)" },
      "&.Mui-focused fieldset": { borderColor: "#3B82F6" },
      backgroundColor: "rgba(255,255,255,0.05)",
      borderRadius: "10px",
    },
    "& .MuiInputLabel-root": { color: "#64748B" },
    "& .MuiInputLabel-root.Mui-focused": { color: "#3B82F6" },
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh",
        background: "linear-gradient(135deg, #0B1220 0%, #172036 50%, #1B3F8B 100%)",
      }}
    >
      <Box
        sx={{
          width: 400,
          borderRadius: "16px",
          p: 5,
          background: "rgba(255,255,255,0.05)",
          backdropFilter: "blur(20px)",
          border: "1px solid rgba(255,255,255,0.1)",
          boxShadow: "0 25px 50px rgba(0,0,0,0.4)",
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", gap: 1.5, mb: 3 }}>
          <HotelOutlined sx={{ fontSize: 36, color: "#60A5FA" }} />
          <Typography variant="h5" sx={{ color: "#F1F5F9", fontWeight: 700 }}>
            Dashyboard
          </Typography>
        </Box>

        <Typography variant="h5" sx={{ color: "#F1F5F9", fontWeight: 700, mb: 0.5 }}>
          Welcome back
        </Typography>
        <Typography variant="body2" sx={{ color: "#64748B", mb: 4 }}>
          Sign in to your admin account
        </Typography>

        <Box
          component="form"
          onSubmit={handleSubmit}
          sx={{ display: "flex", flexDirection: "column", gap: 2.5 }}
        >
          <TextField
            label="Hotel ID"
            value={hotelId}
            onChange={(e) => setHotelId(e.target.value)}
            required
            fullWidth
            autoFocus
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <BadgeOutlined sx={{ color: "#64748B" }} />
                </InputAdornment>
              ),
            }}
            sx={fieldSx}
          />

          <TextField
            label="Password"
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            fullWidth
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <LockOutlined sx={{ color: "#64748B" }} />
                </InputAdornment>
              ),
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    onClick={() => setShowPassword((prev) => !prev)}
                    edge="end"
                    sx={{ color: "#64748B" }}
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
            sx={fieldSx}
          />

          {error && <Alert severity="error">{error}</Alert>}

          <Button
            type="submit"
            variant="contained"
            fullWidth
            disabled={loading}
            sx={{
              mt: 1,
              py: 1.5,
              borderRadius: "10px",
              backgroundColor: "#2563EB",
              fontSize: "0.9rem",
              fontWeight: 700,
              letterSpacing: "0.05em",
              "&:hover": { backgroundColor: "#3B82F6" },
            }}
          >
            {loading ? <CircularProgress size={24} sx={{ color: "white" }} /> : "Sign in"}
          </Button>
        </Box>
      </Box>
    </Box>
  );
}
