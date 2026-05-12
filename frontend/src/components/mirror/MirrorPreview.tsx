import { Box } from "@mui/material";
import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import MirrorDashboard from "./MirrorDashboard";

function MirrorPreview() {
  return (
    <DndProvider backend={HTML5Backend}>
      <Box
        sx={{
          position: "relative",
          width: "100vw",
          height: "100vh",
          overflow: "hidden",
          bgcolor: "#000",
        }}
      >
        {/* Mirror content (the actual dashboard) */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            transform: "scaleX(-1)",
            zIndex: 1,
          }}
        >
          <MirrorDashboard />
        </Box>

        {/* Glass mirror overlay */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            zIndex: 2,
            pointerEvents: "none",
            background: `
              linear-gradient(
                135deg,
                rgba(255,255,255,0.08) 0%,
                rgba(255,255,255,0.02) 30%,
                rgba(0,0,0,0.15) 50%,
                rgba(255,255,255,0.03) 70%,
                rgba(255,255,255,0.06) 100%
              )
            `,
            // Subtle reflection spot in upper-left
            "&::before": {
              content: '""',
              position: "absolute",
              top: "5%",
              left: "10%",
              width: "35%",
              height: "20%",
              background:
                "radial-gradient(ellipse, rgba(255,255,255,0.06) 0%, transparent 70%)",
              transform: "rotate(-15deg)",
            },
          }}
        />

        {/* Mirror frame */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            zIndex: 3,
            pointerEvents: "none",
            border: "12px solid",
            borderImage: `linear-gradient(
              145deg,
              #4a4a4a 0%,
              #8a8a8a 20%,
              #b0b0b0 40%,
              #8a8a8a 60%,
              #4a4a4a 100%
            ) 1`,
          }}
        />
      </Box>
    </DndProvider>
  );
}

export default MirrorPreview;
