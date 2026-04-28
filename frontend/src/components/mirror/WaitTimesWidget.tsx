import { Box, Typography, Paper, Stack } from "@mui/material";
import { widgetTheme } from "../../theme/index.ts";
import { useWaitTimes } from "../../hooks";

interface WaitTimeaProps {
  airport: string;
}

export default function WaitTimeWidget({ airport }: WaitTimeaProps) {
  console.log(airport);
  const {
    data: waitTimes = [],
    error,
    isLoading,
  } = useWaitTimes({
    airport,
  });

  if (error)
    return (
      <Typography
        sx={{
          m: 3,
          opacity: 0.9,
          color: `${widgetTheme.palette.primary.main}`,
        }}
      >
        Error: {error.message}
      </Typography>
    );
  if (isLoading)
    return (
      <Typography
        sx={{
          m: 3,
          opacity: 0.9,
          color: `${widgetTheme.palette.primary.main}`,
        }}
      >
        Loading arrivals info...
      </Typography>
    );

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
        width: "15em",
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
            Waiting Times
          </Typography>
        </Box>
      </Box>
      <Stack spacing={1.2}>
        {waitTimes.slice(0, 5).map((waitTime) => {
          return (
            <Paper
              key={waitTime.queueName}
              sx={{
                p: 1.2,
                borderRadius: 2,
                bgcolor: `${widgetTheme.palette.primary.dark}`,
                color: `${widgetTheme.palette.primary.main}`,
                border: `2px solid ${widgetTheme.palette.primary.light}`,
              }}
            >
              <Stack direction="column" justifyContent="space-between">
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: "space-between",
                  }}
                >
                  <Typography sx={{ fontWeight: 700, fontSize: "0.9rem" }}>
                    {waitTime.queueName ?? "-"}
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "0.7rem",
                      fontWeight: 600,
                      color: "#3b82f6",
                    }}
                  >
                    {waitTime.terminal ?? "-"}
                  </Typography>
                </Box>

                <Box sx={{ mt: 0.5 }}>
                  <Box
                    sx={{
                      fontSize: "0.8rem",
                      display: "flex",
                      justifyContent: "space-between",
                    }}
                  >
                    <Typography sx={{ fontSize: "0.8rem", fontWeight: 600 }}>
                      Current wait:
                    </Typography>
                    <Typography sx={{ fontSize: "0.8rem" }}>
                      {" "}
                      {waitTime.currentProjectedWaitTime ?? "-"} min
                    </Typography>
                  </Box>
                </Box>
              </Stack>
            </Paper>
          );
        })}
      </Stack>
    </Box>
  );
}
