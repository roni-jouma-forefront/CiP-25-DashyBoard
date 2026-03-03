import { useParams } from "react-router";
import { RoomDetailsForm } from "../../components/admin/forms/RoomDetailsForm";
import { RoomMessageForm } from "../../components/admin/forms/RoomMessageForm";
import { Button, Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";
import AlertDialog from "../../components/admin/AlertDialog";
import React from "react";

export default function Room() {
  const { id } = useParams();
  const [dialogOpen, setDialogOpen] = React.useState(false);

  const handleDialog = () => {
    setDialogOpen(true);
  };

  const handleClose = () => {
    setDialogOpen(false);
  };

  return (
    <>
      <Stack
        direction="row"
        spacing={2}
        justifyContent="space-between"
        alignItems="flex-end"
      >
        <Typography variant="h2">Details for room {id}</Typography>
        <Button
          variant="contained"
          color="error"
          onClick={() => handleDialog()}
        >
          Check out
        </Button>
      </Stack>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <RoomDetailsForm />
        <RoomMessageForm />
      </Stack>
      <MessageAccordion />
      <AlertDialog open={dialogOpen} onClose={handleClose} />
    </>
  );
}
