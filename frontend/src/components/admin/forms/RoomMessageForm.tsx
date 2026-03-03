import { Box, Button, Stack, Typography } from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";

export const RoomMessageForm = () => {
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
        Message
      </Typography>
      <Stack spacing={3}>
        <MessageBaseForm />
        <Button variant="contained">Post</Button>
      </Stack>
    </Box>
  );
};
