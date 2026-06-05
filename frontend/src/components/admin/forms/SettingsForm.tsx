import { Box, Button, Stack, TextField, Typography } from "@mui/material";
import { useState } from "react";
import { postHotel } from "../../../services/api/postHotel";

export const SettingsForm = () => {
  const [formData, setFormData] = useState({ id: "", name: "", icaoCode: "" });
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSuccess(false);
    setError(null);

    try {
      await postHotel(formData);
      setSuccess(true);
      setFormData({ id: "", name: "", icaoCode: "" });
    } catch (error) {
      console.error("Error posting hotel:", error);
      setError(error instanceof Error ? error.message : "Failed to save hotel");
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        width: "500px",
        p: 3,
        borderRadius: 3,
        boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
        border: "1px solid rgba(0,0,0,0.06)",
        background: "white",
      }}
    >
      <Typography variant="h5" mb={3}>
        Hotel Details
      </Typography>
      <Stack spacing={2}>
        <TextField
          label="Hotel Name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          label="Hotel Location (ICAO Code)"
          name="icaoCode"
          value={formData.icaoCode}
          onChange={handleChange}
          fullWidth
          required
          slotProps={{ htmlInput: { maxLength: 4 } }}
          helperText="Must be exactly 4 letters"
          error={formData.icaoCode.length > 0 && !/^[A-Za-z]{4}$/.test(formData.icaoCode)}
        />
        <TextField type="number" label="Number of Rooms" />
        {error && (
          <Typography color="error" variant="body2">
            {error}
          </Typography>
        )}
        {success && (
          <Typography variant="body2" color="success.main">
            Hotel saved successfully
          </Typography>
        )}
        <Button
          type="submit"
          variant="contained"
          disabled={formData.name.length < 5 || !/^[A-Za-z]{4}$/.test(formData.icaoCode)}
        >
          Save
        </Button>
      </Stack>
    </Box>
  );
};
