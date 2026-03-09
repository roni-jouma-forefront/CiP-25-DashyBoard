import { Box } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import { useState } from "react";
import { useDrop } from "react-dnd";
import DraggableWrapper from "./DraggableWrapper";
import WeatherWidget from "./WeatherWidget";
import Watch from "../base/watch";

function MirrorDashboard() {
  const [order, setOrder] = useState([1, 2]);

  const [{ isOver }, drop] = useDrop(() => ({
    accept: "widget",
    drop: (item: { id: number }) => {
      setOrder((prev) => {
        const rest = prev.filter((id) => id !== item.id);
        return [...rest, item.id];
      });
    },
    collect: (monitor) => ({
      isOver: monitor.isOver(),
    }),
  }));

  return (
    <>
      <Box
        sx={{
          position: "relative",
          width: "100%",
          minHeight: "100vh",
          display: "flex",
          justifyContent: "center",
        }}
      >
        <Box
          sx={{
            position: "relative",
            width: "100%",
            height: "100%",
            maxWidth: "100%",
            background: "#fff",
            padding: "120px",
          }}
        >
          <Box
            ref={drop as unknown as React.RefObject<HTMLDivElement>}
            sx={{
              borderImage: "linear-gradient(45deg, #f06, #09f) 1",
              paddingRight: { xs: "1rem", sm: "3rem", md: "10rem" },
              backgroundColor: isOver ? "rgba(0,0,0,0.1)" : "transparent",
              display: "flex",
              flexWrap: "wrap",
              alignContent: "flex-start",
            }}
          >
            {order.map((id) => {
              if (id === 1)
                return (
                  <DraggableWrapper key={1} id={1}>
                    <Watch
                      key={1}
                      location="Stockholm"
                      timeZone="Europe/Stockholm"
                    />
                  </DraggableWrapper>
                );
              if (id === 2)
                return (
                  <DraggableWrapper key={2} id={2}>
                    <WeatherWidget icao="ESSA" />
                  </DraggableWrapper>
                );
            })}
          </Box>
          <Box
            component="svg"
            viewBox="0 0 1200 800"
            xmlns="http://www.w3.org/2000/svg"
            sx={{
              position: "absolute",
              width: "100%",
              top: 0,
              left: 0,
              height: "auto",
              pointerEvents: "none",
            }}
          >
            {/* SVG-ram */}

            <defs>
              <filter id="shadow">
                <feDropShadow
                  dx="0"
                  dy="0"
                  stdDeviation="8"
                  floodColor="#000"
                  floodOpacity="0.35"
                />
              </filter>
            </defs>

            <rect
              x="0"
              y="0"
              width="1200"
              height="800"
              fill="none"
              stroke="url(#gold)"
              filter="url(#shadow)"
            />

            <rect
              x="20"
              y="20"
              width="1160"
              height="760"
              fill="none"
              stroke="#161e88"
              strokeWidth="60"
            />

            <rect
              x="45"
              y="45"
              width="1110"
              height="710"
              fill="none"
              stroke="#b9e1fb"
              strokeWidth="4"
            />
          </Box>
        </Box>
      </Box>
    </>
  );
}

export default MirrorDashboard;
