import {
  Box,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { useState } from "react";
import type { Staff } from "../../../types/types";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";

const mockStaff: Staff[] = [
  { name: "Emmi Quirin" },
  { name: "Anna C Hallberg" },
  { name: "Nikita Sjölander" },
];

export const AddStaffForm = () => {
  const [staffList, setStaffList] = useState(mockStaff);
  const [formData, setFormData] = useState<Staff>({ name: "" });

  const handleChage = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    setStaffList((prev) => [...prev, formData]);
    setFormData({ name: "" });
  };

  return (
    <Stack
      direction="column"
      spacing={2}
      sx={{
        width: "500px",
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
    >
      <Box component="form" sx={{}} onSubmit={handleSubmit}>
        <Typography variant="h5" mb={3}>
          Staff
        </Typography>
        <Stack flexDirection="row" spacing={3} alignItems="stretch">
          <TextField
            label="Name"
            sx={{ flex: 1 }}
            name="name"
            value={formData.name}
            onChange={handleChage}
          />
          <Button variant="contained" type="submit" sx={{ mt: "0 !important" }}>
            Add Staff
          </Button>
        </Stack>
      </Box>

      <List>
        {staffList.map((staff, index) => (
          <ListItem
            key={index}
            secondaryAction={
              <IconButton edge="end" aria-label="delete">
                <DeleteIcon />
              </IconButton>
            }
          >
            <IconButton edge="end" aria-label="delete">
              <EditIcon />
            </IconButton>
            <ListItemText />
            {staff.name}
          </ListItem>
        ))}
      </List>
    </Stack>
  );
};
