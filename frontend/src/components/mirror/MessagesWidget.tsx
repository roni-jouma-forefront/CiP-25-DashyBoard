import { Box, Typography, Paper, Stack } from "@mui/material";
import { widgetTheme } from "../../theme/index.ts";

// interface MessageProps {
//     bookingId: number,
//     title: string,
//     content: string,
// }

export default function MessagesWidget() {
  const messageMocks = [
    {
      key: 1,
      postAt: 13.45,
      bookingId: "Kalle",
      title: "Meddelande 1",
      content:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    },
    {
      key: 2,
      postAt: 13.45,
      bookingId: "Mary",
      title: "Meddelande 2",
      content:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    },
    {
      key: 3,
      postAt: 13.45,
      bookingId: "Lisa",
      title: "Meddelande 3",
      content:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    },
  ];

  // const { data: messages, error, isLoading } = useMessages();

  //   if (error)
  //     return (
  //       <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
  //         Error: {error.message}
  //       </Typography>
  //     );
  //   if (isLoading)
  //     return (
  //       <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
  //         Loading arrivals info...
  //       </Typography>
  //     );

  return (
    <Box
      sx={{
        position: "relative",
        p: 2,
        m: 2,
        borderRadius: 2,
        border: `5px solid ${widgetTheme.palette.primary.main}`,
        boxShadow: 1,
        color: `${widgetTheme.palette.primary.main}`,
        backgroundColor: `${widgetTheme.palette.primary.dark}`,
        width: "20em",
      }}
    >
      <Box sx={{ mb: 2 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <Typography sx={{ fontSize: "1.4rem", fontWeight: 700 }}>
            Messages
          </Typography>
        </Box>
      </Box>
      <Stack spacing={1.2}>
        {messageMocks.map((message) => {
          return (
            <Paper
              sx={{
                p: 1.2,
                borderRadius: 2,
                bgcolor: `${widgetTheme.palette.primary.dark}`,
                color: `${widgetTheme.palette.primary.main}`,
                border: `2px solid ${widgetTheme.palette.primary.light}`,
              }}
            >
              <Stack direction="column">
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignContent: "center",
                  }}
                >
                  <Typography sx={{ fontSize: "1.2rem", fontWeight: 700 }}>
                    {message.title}
                  </Typography>
                  <Typography
                    sx={{
                      display: "flex",
                      alignContent: "center",
                    }}
                  >
                    {message.postAt}
                  </Typography>
                </Box>

                <Box>
                  <Typography sx={{ fontSize: "0.9rem", pt: 1 }}>
                    {message.content}
                  </Typography>
                </Box>
                <Box
                  sx={{
                    fontSize: "0.8rem",
                    textAlign: "right",
                    pt: 2,
                  }}
                >
                  <Typography>{message.bookingId}</Typography>
                </Box>
              </Stack>
            </Paper>
          );
        })}
      </Stack>
    </Box>
  );
}
