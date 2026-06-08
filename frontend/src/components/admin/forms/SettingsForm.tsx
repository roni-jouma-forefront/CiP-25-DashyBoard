import { Box, Button, Stack, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { GetHotel } from "../../../services/api/GetHotel";
import { updateHotel } from "../../../services/api/updateHotel";

export const SettingsForm = () => {
  const hotelId = import.meta.env.VITE_HOTEL_ID;
  const missingHotelId = !hotelId;
  const [formData, setFormData] = useState({ id: "", name: "", iataCode: "" });
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (missingHotelId) {
      return;
    }

    GetHotel(hotelId)
      .then((hotel) => {
        setFormData({
          id: hotel.id,
          name: hotel.name,
          iataCode: hotel.icaoCode,
        });
      })
      .catch((loadError) => {
        console.error("Error loading hotel settings:", loadError);
        setError("Failed to load hotel settings");
      });
  }, [hotelId, missingHotelId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const nextValue =
      name === "iataCode" ? value.toUpperCase().replace(/[^A-Z]/g, "") : value;

    setFormData((prev) => ({ ...prev, [name]: nextValue }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSuccess(false);
    setError(null);

    if (!hotelId) {
      setError("VITE_HOTEL_ID is missing.");
      return;
    }

    try {
      const normalizedFormData = {
        ...formData,
        name: formData.name.trim(),
        iataCode: formData.iataCode.trim().toUpperCase(),
      };

      await updateHotel(hotelId, normalizedFormData);

      setSuccess(true);
      setFormData((prev) => ({
        ...prev,
        name: normalizedFormData.name,
        iataCode: normalizedFormData.iataCode,
      }));
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
          label="Hotel Location (IATA code)"
          name="iataCode"
          value={formData.iataCode}
          onChange={handleChange}
          fullWidth
          required
          slotProps={{ htmlInput: { maxLength: 3 } }}
          helperText="Must be exactly 3 letters"
          error={
            formData.iataCode.length > 0 &&
            !/^[A-Za-z]{3}$/.test(formData.iataCode)
          }
        />
        {(error || missingHotelId) && (
          <Typography color="error" variant="body2">
            {error ?? "VITE_HOTEL_ID is missing."}
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
          disabled={
            missingHotelId ||
            formData.name.trim().length < 2 ||
            !/^[A-Za-z]{3}$/.test(formData.iataCode)
          }
        >
          Save
        </Button>
      </Stack>
    </Box>
  );
};
