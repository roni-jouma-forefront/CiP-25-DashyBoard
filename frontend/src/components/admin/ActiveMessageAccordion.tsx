import {
  Box,
  Typography,
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Stack,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

type MessageType = "all" | "checkingOut" | "checkingIn";

type Message = {
  id: string;
  messageType: MessageType;
  title: string;
  message: string;
  postDate: string | null;
  postTime: string | null;
  deleteDate: string | null;
  deleteTime: string | null;
};

const mockMessages: Message[] = [
  {
    id: "1",
    messageType: "all",
    title: "Muffins for breakfast",
    message: "Today we offer muffins for breakfast at the front desk.",
    postDate: "26-01-25",
    postTime: "06:00",
    deleteDate: "26-01-25",
    deleteTime: "10:00",
  },
  {
    id: "2",
    messageType: "checkingOut",
    title: "Thank you for your stay",
    message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
    postDate: "26-01-25",
    postTime: "06:00",
    deleteDate: "26-01-25",
    deleteTime: "15:00",
  },
];

const badgeStyle = {
  display: "inline-block",
  lineHeight: 1,
  backgroundColor: "pink",
  py: 0.5,
  px: 1.5,
  borderRadius: 3,
  fontWeight: 600,
  fontSize: "0.75rem",
};

export const ActiveMessageAccordion = () => {
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
      <Typography variant="h5">Active Messages</Typography>

      {mockMessages.map((msg) => (
        <Accordion key={msg.id}>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls={`panel-${msg.id}-content`}
            id={`panel-${msg.id}-header`}
          >
            <Stack direction="row" spacing={2} alignItems="center" justifyContent="space-between" width={1}>
              <Typography component="span" sx={{ flex: 1 }}>
                {msg.title}
              </Typography>
              <Stack direction="row" spacing={2} alignItems="center">
                <Typography component="span" sx={badgeStyle}>
                  Posted: {msg.postDate}, {msg.postTime}
                </Typography>
                <Button value={msg.id}>Delete</Button>
              </Stack>
            </Stack>
          </AccordionSummary>
          <AccordionDetails>
            <Stack direction="row" spacing={2}  alignItems="flex-start">
            <Typography flex={1}>{msg.message}</Typography>
              <Typography component="span" sx={badgeStyle} >
                Deletes: {msg.deleteDate}, {msg.deleteTime}
              </Typography>
            </Stack>

          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
