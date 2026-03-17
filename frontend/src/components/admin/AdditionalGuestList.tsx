import {
  IconButton,
  List,
  ListItem,
  ListItemText,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import type { AdditionalGuest } from "../../types/types";

interface AdditionalGuestProps {
  guests: AdditionalGuest[];
}

export const AdditionalGuestList = ({ guests }: AdditionalGuestProps) => {

  return (
    <>
      <List>
        {guests.map((guest, index) => (
          <ListItem
            key={index}
            secondaryAction={
              <IconButton edge="end" aria-label="delete">
                <DeleteIcon />
              </IconButton>
            }
          >
            <ListItemText />
            {guest.firstName} {guest.lastName}
          </ListItem>
        ))}
      </List>
    </>
  );
};
