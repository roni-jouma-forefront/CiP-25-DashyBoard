import { useState } from "react";
import { useNavigate } from "react-router";
import {
  Box,
  Button,
  CircularProgress,
  IconButton,
  InputAdornment,
  Stack,
  TextField,
  Typography,
  Alert,
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { theme } from "../theme";
import { login } from "../services/api/login";
import { useAuth } from "../context/AuthContext";

export default function LoginPage() {
  const navigate = useNavigate();
  const { setAuth } = useAuth();

  const [hotelId, setHotelId] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!hotelId.trim()) {
      setError("Hotel ID är obligatoriskt");
      return;
    }
    if (!password) {
      setError("Lösenord är obligatoriskt");
      return;
    }

    setIsLoading(true);
    try {
      const result = await login({ hotelId, password });
      setAuth(result.token, result.admin);
      navigate("/admin", { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: theme.palette.topbar.background,
      }}
    >
      <Box
        component="form"
        onSubmit={handleSubmit}
        noValidate
        sx={{
          width: "100%",
          maxWidth: 420,
          bgcolor: theme.palette.sidebar.background,
          borderRadius: 2,
          p: 4,
          boxShadow: "0 8px 32px rgba(0,0,0,0.4)",
        }}
      >
        <Stack spacing={3}>
          <Box>
            <Typography
              variant="h4"
              sx={{ color: "#FFFFFF", fontWeight: 700, mb: 0.5 }}
            >
              DashyBoard
            </Typography>
            <Typography
              variant="body2"
              sx={{ color: theme.palette.topbar.text }}
            >
              Logga in på adminpanelen
            </Typography>
          </Box>

          {error && (
            <Alert severity="error" sx={{ borderRadius: 1 }}>
              {error}
            </Alert>
          )}

          <TextField
            label="Hotel ID"
            value={hotelId}
            onChange={(e) => setHotelId(e.target.value)}
            autoFocus
            fullWidth
            placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
            disabled={isLoading}
            slotProps={{
              inputLabel: { style: { color: theme.palette.topbar.text } },
              input: {
                style: {
                  color: "#FFFFFF",
                },
              },
            }}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#2D3F5E" },
                "&:hover fieldset": { borderColor: theme.palette.primary.main },
                "&.Mui-focused fieldset": {
                  borderColor: theme.palette.primary.main,
                },
              },
            }}
          />

          <TextField
            label="Lösenord"
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            fullWidth
            disabled={isLoading}
            slotProps={{
              inputLabel: { style: { color: theme.palette.topbar.text } },
              input: {
                style: { color: "#FFFFFF" },
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      onClick={() => setShowPassword((prev) => !prev)}
                      edge="end"
                      sx={{ color: theme.palette.topbar.text }}
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              },
            }}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#2D3F5E" },
                "&:hover fieldset": { borderColor: theme.palette.primary.main },
                "&.Mui-focused fieldset": {
                  borderColor: theme.palette.primary.main,
                },
              },
            }}
          />

          <Button
            type="submit"
            variant="contained"
            fullWidth
            disabled={isLoading}
            size="large"
            sx={{ mt: 1, height: 48 }}
          >
            {isLoading ? (
              <CircularProgress size={22} color="inherit" />
            ) : (
              "Logga in"
            )}
          </Button>
        </Stack>
      </Box>
    </Box>
  );
}
