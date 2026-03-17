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
import type { MessageBackend, MessageUI } from "../../types/message.types";
import { useState } from "react";
import type { MsgStatus } from "../../types/theme.types";


const mockMessages: MessageUI[] = [
  {
    id: "1",
    messageScope: "all",
    title: "Muffins for breakfast",
    content: "Today we offer muffins for breakfast at the front desk.",
    status: "posted",
    postDateTime: "26-01-25 - 06:00",
    deleteDateTime: "26-01-25 - 06:00",
  },
  {
    id: "2",
    messageScope: "checkingOut",
    title: "Thank you for your stay",
    content:
      "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
    status: "pending",
    postDateTime: "26-01-25 - 06:00",
    deleteDateTime: "26-01-25 - 10:00",
  },
];

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

export const MessageAccordion = () => {
  const [mockData, setMockData] = useState(mockMessages);
  const [editMode, setEditMode] = useState("");
  const [formData, setFormData] = useState<MessageBackend>({
    id: "",
    messageScope: "all",
    title: "",
    content: "",
    postDate: "",
    postTime: "",
    deleteDate: "",
    deleteTime: "",
    isActive: false,
  });

  const handleEdit = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    e.preventDefault();

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSave = (id: string) => {
    setMockData((prev) =>
      prev.map((msg) => {
        if (msg.id === id) {
          return {
            ...msg,
            title: formData.title,
            content: formData.content,
          };
        } else {
          return msg;
        }
      }),
    );
    setEditMode("");
  };

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

      {mockData.map((msg) => (
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
              {editMode === msg.id ? (
                <>
                  <TextField
                    label="Title"
                    name="title"
                    fullWidth
                    value={formData.title}
                    onChange={handleEdit}
                  />
                  <Button
                    onClick={() => {
                      handleSave(msg.id);
                    }}
                  >
                    Save
                  </Button>
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
            {editMode === msg.id ? (
              <TextField
                label="Content"
                name="content"
                multiline
                rows={4}
                fullWidth
                value={formData.content}
                onChange={handleEdit}
              />
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
                      Delets {msg.deleteDateTime}
                    </Typography>
                  </Stack>
                  <Stack direction="row" spacing={2}>
                    <Button
                      onClick={() => {
                        setEditMode(msg.id);
                        setFormData((prev) => ({
                          ...prev,
                          title: msg.title,
                          content: msg.content,
                        }));
                      }}
                    >
                      Edit
                    </Button>
                    <Button value={msg.id}>Delete</Button>
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
