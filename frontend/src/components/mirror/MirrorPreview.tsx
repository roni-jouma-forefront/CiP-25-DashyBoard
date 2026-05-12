import { Box } from "@mui/material";
import { useEffect, useRef } from "react";
import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import MirrorDashboard from "./MirrorDashboard";

function MirrorPreview() {
  const videoRef = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    const video = videoRef.current;
    navigator.mediaDevices
      .getUserMedia({ video: true })
      .then((stream) => {
        if (video) {
          video.srcObject = stream;
        }
      })
      .catch(() => {
        // Webcam not available – mirror will just show dark background
      });

    return () => {
      if (video?.srcObject) {
        (video.srcObject as MediaStream).getTracks().forEach((t) => t.stop());
      }
    };
  }, []);

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
        {/* Webcam feed as the "reflection" – flipped horizontally like a real mirror */}
        <Box
          component="video"
          ref={videoRef}
          autoPlay
          muted
          playsInline
          sx={{
            position: "absolute",
            inset: 0,
            width: "100%",
            height: "100%",
            objectFit: "cover",
            transform: "scaleX(-1)",
            zIndex: 0,
            opacity: 0.35,
            filter: "brightness(0.6) contrast(1.1)",
          }}
        />

        {/* Widgets on top – glowing through the mirror */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            zIndex: 1,
          }}
        >
          <MirrorDashboard />
        </Box>

        {/* Subtle glass reflection highlights */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            zIndex: 2,
            pointerEvents: "none",
            background: `
              radial-gradient(
                ellipse 70% 40% at 25% 12%,
                rgba(255,255,255,0.10) 0%,
                transparent 60%
              ),
              radial-gradient(
                ellipse 50% 30% at 80% 85%,
                rgba(255,255,255,0.05) 0%,
                transparent 50%
              )
            `,
          }}
        />

        {/* Mirror frame – wooden style like the reference */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            zIndex: 3,
            pointerEvents: "none",
            borderWidth: "18px",
            borderStyle: "solid",
            borderColor: "#5a3e28",
            borderImage: `linear-gradient(
              145deg,
              #3a2515 0%,
              #6b4830 20%,
              #8b6340 40%,
              #a07850 50%,
              #8b6340 60%,
              #6b4830 80%,
              #3a2515 100%
            ) 1`,
            boxShadow:
              "inset 0 0 40px rgba(0,0,0,0.4), 0 0 50px rgba(0,0,0,0.6)",
          }}
        />
      </Box>
    </DndProvider>
  );
}

export default MirrorPreview;
