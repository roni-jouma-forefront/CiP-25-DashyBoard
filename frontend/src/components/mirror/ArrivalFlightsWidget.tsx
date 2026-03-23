import { useState, useEffect } from "react";
import {
  Box,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
  Paper,
  Container,
} from "@mui/material";
import FlightLandIcon from "@mui/icons-material/FlightLand";
import { createTheme, ThemeProvider } from "@mui/material/styles";

const theme = createTheme({
  palette: {
    mode: "dark",
    background: {
      default: "#0a0e1a",
      paper: "#111827",
    },
    primary: {
      main: "#38bdf8",
    },
    text: {
      primary: "#f1f5f9",
      secondary: "#94a3b8",
    },
  },
  typography: {
    fontFamily: "'JetBrains Mono', 'Courier New', monospace",
  },
  components: {
    MuiTableCell: {
      styleOverrides: {
        root: {
          borderBottom: "1px solid rgba(255,255,255,0.06)",
          padding: "14px 20px",
        },
        head: {
          color: "#64748b",
          fontSize: "0.7rem",
          letterSpacing: "0.12em",
          textTransform: "uppercase",
          fontWeight: 600,
        },
      },
    },
  },
});

const STATUS = {
  LANDED: { label: "Landed", color: "success" },
  ON_TIME: { label: "On Time", color: "info" },
  DELAYED: { label: "Delayed", color: "warning" },
  BOARDING: { label: "Boarding", color: "primary" },
  ARRIVED: { label: "Arrived", color: "success" },
};

const mockFlights = [
  {
    id: "SK123",
    route: "STHLM → OSLO",
    terminal: "5E",
    scheduled: "07:45",
    status: STATUS.LANDED,
    gate: "G22",
    highlighted: true,
  },
  {
    id: "LH801",
    route: "BERLIN → STHLM",
    terminal: "2A",
    scheduled: "08:10",
    status: STATUS.ON_TIME,
    gate: "A04",
  },
  {
    id: "DY450",
    route: "OSLO → STHLM",
    terminal: "4C",
    scheduled: "08:35",
    status: STATUS.BOARDING,
    gate: "B11",
  },
  {
    id: "AY902",
    route: "HELSINKI → STHLM",
    terminal: "5B",
    scheduled: "09:00",
    status: STATUS.ON_TIME,
    gate: "C07",
  },
  {
    id: "BA777",
    route: "LONDON → STHLM",
    terminal: "5E",
    scheduled: "09:20",
    status: STATUS.DELAYED,
    gate: "G18",
  },
  {
    id: "AF123",
    route: "PARIS → STHLM",
    terminal: "2D",
    scheduled: "09:50",
    status: STATUS.ON_TIME,
    gate: "A09",
  },
  {
    id: "KL234",
    route: "AMSTERDAM → STHLM",
    terminal: "5C",
    scheduled: "10:15",
    status: STATUS.ON_TIME,
    gate: "C14",
  },
  {
    id: "TK178",
    route: "ISTANBUL → STHLM",
    terminal: "5A",
    scheduled: "10:40",
    status: STATUS.DELAYED,
    gate: "G05",
  },
  {
    id: "LO456",
    route: "WARSAW → STHLM",
    terminal: "2B",
    scheduled: "11:05",
    status: STATUS.ON_TIME,
    gate: "A16",
  },
  {
    id: "SK999",
    route: "COPENHAGEN → STHLM",
    terminal: "5E",
    scheduled: "11:30",
    status: STATUS.ON_TIME,
    gate: "G31",
  },
];

function Clock() {
  const [time, setTime] = useState(new Date());
  useEffect(() => {
    const t = setInterval(() => setTime(new Date()), 1000);
    return () => clearInterval(t);
  }, []);
  return (
    <Typography
      sx={{
        fontFamily: "'JetBrains Mono', monospace",
        fontSize: "1.5rem",
        color: "#38bdf8",
        letterSpacing: "0.05em",
        fontWeight: 700,
      }}
    >
      {time.toLocaleTimeString("sv-SE")}
    </Typography>
  );
}

export default function ArrivalsBoard() {
  return (
    <ThemeProvider theme={theme}>
      <Box
        sx={{
          minHeight: "100vh",
          bgcolor: "#0a0e1a",
          backgroundImage:
            "radial-gradient(ellipse at 20% 0%, rgba(56,189,248,0.07) 0%, transparent 60%), radial-gradient(ellipse at 80% 100%, rgba(99,102,241,0.05) 0%, transparent 60%)",
          py: 5,
          px: 2,
        }}
      >
        <Container maxWidth="md">
          {/* Header */}
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              mb: 4,
            }}
          >
            <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
              <FlightLandIcon
                sx={{ color: "#38bdf8", fontSize: 28, transform: "scaleX(-1)" }}
              />
              <Typography
                sx={{
                  fontFamily: "'JetBrains Mono', monospace",
                  fontSize: "1.1rem",
                  fontWeight: 700,
                  letterSpacing: "0.25em",
                  color: "#f1f5f9",
                  textTransform: "uppercase",
                }}
              >
                Arrivals
              </Typography>
            </Box>
            <Clock />
          </Box>

          {/* Highlighted flight */}
          {mockFlights
            .filter((f) => f.highlighted)
            .map((f) => (
              <Paper
                key={f.id}
                sx={{
                  mb: 3,
                  p: 3,
                  bgcolor: "rgba(56,189,248,0.07)",
                  border: "1px solid rgba(56,189,248,0.25)",
                  borderRadius: 2,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  backdropFilter: "blur(8px)",
                }}
              >
                <Box>
                  <Typography
                    sx={{
                      fontFamily: "'JetBrains Mono', monospace",
                      fontSize: "1.4rem",
                      fontWeight: 800,
                      color: "#38bdf8",
                      letterSpacing: "0.08em",
                    }}
                  >
                    {f.id}
                  </Typography>
                  <Typography
                    sx={{
                      color: "#94a3b8",
                      fontSize: "0.85rem",
                      letterSpacing: "0.05em",
                      mt: 0.5,
                    }}
                  >
                    {f.route}
                  </Typography>
                </Box>
                <Box sx={{ textAlign: "right" }}>
                  <Typography
                    sx={{
                      color: "#64748b",
                      fontSize: "0.7rem",
                      letterSpacing: "0.1em",
                      mb: 0.5,
                    }}
                  >
                    TERMINAL
                  </Typography>
                  <Typography
                    sx={{
                      fontFamily: "'JetBrains Mono', monospace",
                      fontSize: "2rem",
                      fontWeight: 700,
                      color: "#f1f5f9",
                      lineHeight: 1,
                    }}
                  >
                    {f.terminal}
                  </Typography>
                </Box>
              </Paper>
            ))}

          {/* Table */}
          <TableContainer
            component={Paper}
            sx={{
              bgcolor: "#111827",
              border: "1px solid rgba(255,255,255,0.06)",
              borderRadius: 2,
              overflow: "hidden",
            }}
          >
            <Table>
              <TableHead>
                <TableRow sx={{ bgcolor: "rgba(255,255,255,0.02)" }}>
                  <TableCell>Flight</TableCell>
                  <TableCell>Route</TableCell>
                  <TableCell>Sched.</TableCell>
                  <TableCell>Terminal</TableCell>
                  <TableCell>Gate</TableCell>
                  <TableCell>Status</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {mockFlights
                  .filter((f) => !f.highlighted)
                  .map((flight, i) => (
                    <TableRow
                      key={flight.id}
                      sx={{
                        "&:hover": {
                          bgcolor: "rgba(56,189,248,0.04)",
                        },
                        transition: "background 0.15s",
                        animationDelay: `${i * 40}ms`,
                      }}
                    >
                      <TableCell>
                        <Typography
                          sx={{
                            fontFamily: "'JetBrains Mono', monospace",
                            fontWeight: 700,
                            fontSize: "0.9rem",
                            color: "#e2e8f0",
                            letterSpacing: "0.05em",
                          }}
                        >
                          {flight.id}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Typography
                          sx={{ fontSize: "0.85rem", color: "#cbd5e1" }}
                        >
                          {flight.route}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Typography
                          sx={{
                            fontFamily: "'JetBrains Mono', monospace",
                            fontSize: "0.85rem",
                            color: "#94a3b8",
                          }}
                        >
                          {flight.scheduled}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Typography
                          sx={{
                            fontFamily: "'JetBrains Mono', monospace",
                            fontWeight: 700,
                            fontSize: "0.9rem",
                            color: "#38bdf8",
                          }}
                        >
                          {flight.terminal}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Typography
                          sx={{
                            fontFamily: "'JetBrains Mono', monospace",
                            fontSize: "0.8rem",
                            color: "#64748b",
                          }}
                        >
                          {flight.gate}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Chip
                          size="small"
                          sx={{
                            fontFamily: "'JetBrains Mono', monospace",
                            fontSize: "0.68rem",
                            letterSpacing: "0.05em",
                            fontWeight: 600,
                            height: 22,
                          }}
                        />
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </TableContainer>

          <Typography
            sx={{
              mt: 3,
              textAlign: "center",
              fontSize: "0.65rem",
              color: "#334155",
              letterSpacing: "0.15em",
              textTransform: "uppercase",
            }}
          >
            Stockholm Arlanda Airport · ARN · Information updates every 60s
          </Typography>
        </Container>
      </Box>
    </ThemeProvider>
  );
}
