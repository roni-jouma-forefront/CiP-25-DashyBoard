import {
  Box,
  Typography,
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Stack,
  TextField,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { theme } from "../../theme";
import type { MsgStatus } from "../../types/theme.types";
import type { MessageBackend, MessageUI } from "../../types/message.types";
import type React from "react";
import { DateTimePicker } from "@mui/x-date-pickers";
import dayjs from "dayjs";
import type { DateTime } from "../../types/types";

const badgeStyle = (status: MsgStatus) => ({
  display: "inline-block",
  lineHeight: 1,
  backgroundColor: theme.palette.msgStatus[status].background,
  color: theme.palette.msgStatus[status].text,
  border: 1,
  BorderColor: theme.palette.msgStatus[status].border,
  py: 0.5,
  px: 1.5,
  borderRadius: 3,
  fontWeight: 600,
  fontSize: "0.75rem",
  textTransform: "uppercase",
});

interface MessageAccordionProps {
  messages: MessageUI[];
  isLoading: boolean;
  error: boolean;
  editingId: string;
  formData: MessageBackend;
  startEdit: (msg: MessageUI) => void;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  handleDateTimeChange: ({ field, value }: DateTime) => void;
  saveEdit: (id: string) => void;
  cancelEdit: () => void;
  handleDelete: (id: string) => void;
}

export const MessageAccordion = ({
  messages,
  isLoading,
  error,
  editingId,
  formData,
  startEdit,
  handleChange,
  handleDateTimeChange,
  saveEdit,
  cancelEdit,
  handleDelete,
}: MessageAccordionProps) => {
  if (isLoading) return <Typography>Loading messages...</Typography>;
  if (error)
    return <Typography color="error">Error loading messages</Typography>;

  return (
    <Box
      sx={{
        display: "flex",
        gap: 2,
        flexDirection: "column",
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
    >
      <Typography variant="h5">Messages</Typography>

      {messages.map((msg) => (
        <Accordion key={msg.id} component="form">
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls={`panel-${msg.id}-content`}
            id={`panel-${msg.id}-header`}
          >
            <Stack
              direction="row"
              spacing={2}
              alignItems="center"
              justifyContent="space-between"
              width={1}
              marginRight={2}
            >
              {editingId === msg.id ? (
                <>
                  <TextField
                    label="Title"
                    name="title"
                    fullWidth
                    value={formData.title}
                    onChange={handleChange}
                  />
                </>
              ) : (
                <>
                  <Typography component="span" sx={{ flex: 1 }}>
                    {msg.title}
                  </Typography>
                  <Stack direction="row" spacing={2} alignItems="center">
                    <Typography component="span" sx={badgeStyle(msg.status)}>
                      {msg.status}
                    </Typography>
                  </Stack>
                </>
              )}
            </Stack>
          </AccordionSummary>
          <AccordionDetails>
            {editingId === msg.id ? (
              <Stack spacing={2}>
                <TextField
                  label="Content"
                  name="content"
                  multiline
                  rows={4}
                  fullWidth
                  value={formData.content}
                  onChange={handleChange}
                />
                <Box sx={{ display: "flex", gap: 3 }}>
                  <DateTimePicker
                    label="Post at"
                    onChange={(value) =>
                      handleDateTimeChange({
                        field: "post",
                        value,
                      })
                    }
                    value={formData.postAt ? dayjs(formData.postAt) : null}
                    sx={{ flex: 1 }}
                  />
                </Box>
                <Box sx={{ display: "flex", gap: 3 }}>
                  <DateTimePicker
                    label="Expires at"
                    onChange={(value) =>
                      handleDateTimeChange({
                        field: "expires",
                        value,
                      })
                    }
                    value={
                      formData.expiresAt ? dayjs(formData.expiresAt) : null
                    }
                    sx={{ flex: 1 }}
                  />
                </Box>
                <Stack direction="row" gap={2} width="100%">
                  <Button
                    variant="contained"
                    sx={{ flex: 1 }}
                    onClick={() => {
                      cancelEdit();
                    }}
                  >
                    Cancel
                  </Button>
                  <Button
                    variant="contained"
                    sx={{ flex: 1 }}
                    onClick={() => {
                      saveEdit(msg.id);
                    }}
                  >
                    Save
                  </Button>
                </Stack>
              </Stack>
            ) : (
              <>
                <Stack
                  direction="row"
                  spacing={2}
                  alignItems="center"
                  justifyContent="space-between"
                  paddingBottom={2}
                  paddingTop={1}
                  borderBottom={1}
                  borderColor={theme.palette.divider}
                >
                  <Stack direction="row" spacing={2} alignItems="flex-start">
                    <Typography component="span" sx={badgeStyle(msg.status)}>
                      {msg.status} {msg.postDateTime}
                    </Typography>
                    <Typography component="span" sx={badgeStyle("delete")}>
                      Expires {msg.expiresAtDateTime}
                    </Typography>
                  </Stack>
                  <Stack direction="row" spacing={2}>
                    <Button
                      onClick={() => {
                        startEdit(msg);
                      }}
                    >
                      Edit
                    </Button>
                    <Button
                      onClick={() => {
                        handleDelete(msg.id);
                      }}
                      value={msg.id}
                    >
                      Delete
                    </Button>
                  </Stack>
                </Stack>
                <Stack
                  direction="row"
                  spacing={2}
                  alignItems="flex-start"
                  paddingTop={2}
                >
                  <Typography flex={1}>{msg.content}</Typography>
                </Stack>
              </>
            )}
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
