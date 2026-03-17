import {
  Box,
  Button,
  MenuItem,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers";
import React, { useState } from "react";
import { AdditionalGuestFields } from "./AdditionalGuestFields";
import { AdditionalGuestList } from "../AdditionalGuestList";
import type { AdditionalGuest } from "../../../types/types";

interface GuestFormData {
  firstName: string;
  lastName: string;
  title: string;
  departureFligth: string;
  departureDate: string;
  additionalGuests: AdditionalGuest[];
}

export const RoomDetailsForm = () => {
  const [editMode, setEditMode] = useState(false);
  const [formData, setFormData] = useState<GuestFormData>({
    firstName: "",
    lastName: "",
    title: "",
    departureFligth: "",
    departureDate: "",
    additionalGuests: [],
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  return (
    <Box
      component="form"
      sx={{
        flex: 1,
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
    >
      <Typography variant="h5" mb={3}>
        Room Details
      </Typography>
      <Stack>
        <Typography variant="body2">First Name:</Typography>
        {editMode ? (
          <TextField
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            fullWidth
            required
            sx={{ mt: 1 }}
          />
        ) : (
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {formData.firstName}
          </Typography>
        )}
        <Typography sx={{ mt: 3 }} variant="body2">
          Last Name:
        </Typography>
        {editMode ? (
          <TextField
            sx={{ mt: 1 }}
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            fullWidth
            required
          />
        ) : (
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {formData.lastName}
          </Typography>
        )}
        <Typography sx={{ mt: 3 }} variant="body2">
          Title:
        </Typography>
        {editMode ? (
          <TextField
            select
            name="title"
            value={formData.title}
            onChange={handleChange}
            required
            fullWidth
            sx={{ mt: 1 }}
          >
            <MenuItem value="Mx">Mx</MenuItem>
            <MenuItem value="Mr">Mr</MenuItem>
            <MenuItem value="Mrs">Mrs</MenuItem>
            <MenuItem value="Ms">Ms</MenuItem>
          </TextField>
        ) : (
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {formData.title}
          </Typography>
        )}
        <Typography sx={{ mt: 3 }} variant="body2">
          Depature Flight:
        </Typography>
        {editMode ? (
          <TextField
            name="departureFligth"
            value={formData.departureFligth}
            onChange={handleChange}
            fullWidth
            required
            sx={{ mt: 1 }}
          />
        ) : (
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {formData.departureFligth}
          </Typography>
        )}
        <Typography sx={{ mt: 3 }} variant="body2">
          Departure Date:
        </Typography>
        {editMode ? (
          <DatePicker
            name="departureDate"
            // value={formData.departureDate}
            // onChange={handleChange}
            label="Departure date"
            sx={{ mt: 1 }}
          />
        ) : (
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {formData.departureFligth}
          </Typography>
        )}
        {editMode ? (
          <>
            <AdditionalGuestFields />
            <AdditionalGuestList guests={formData.additionalGuests} />
          </>
        ) : (
          <>
            <Typography sx={{ mt: 3 }} variant="body2">
              Additional Guest(s)
            </Typography>
            <AdditionalGuestList guests={formData.additionalGuests} />
          </>
        )}
        {editMode ? (
          <Button
            variant="contained"
            onClick={() => setEditMode(false)}
            sx={{ mt: 3 }}
          >
            Save
          </Button>
        ) : (
          <Button
            variant="contained"
            onClick={() => setEditMode(true)}
            sx={{ mt: 3 }}
          >
            Edit
          </Button>
        )}
      </Stack>
    </Box>
  );
};
