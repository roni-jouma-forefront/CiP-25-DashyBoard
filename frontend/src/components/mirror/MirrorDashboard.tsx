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
          flexGrow: 1,
        }}
      >
        <AppBar
          position="fixed"
          sx={{
            background: "rgb(0, 45, 135))",
            top: 0,
            left: 0,
            right: 0,
            display: "flex",
            justifyContent: "row",
          }}
        >
          <Toolbar>
            <Typography variant="h6" component="div">
              Room 123
            </Typography>
          </Toolbar>
        </AppBar>
      </Box>

      <Box
        ref={drop as unknown as React.RefObject<HTMLDivElement>}
        sx={{
          borderImage: "linear-gradient(45deg, #f06, #09f) 1",
          paddingRight: { xs: "1rem", sm: "3rem", md: "10rem" },
          backgroundColor: isOver ? "rgba(0,0,0,0.1)" : "transparent",
          display: "flex",
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
                <WeatherWidget icao="ESSA"/>
              </DraggableWrapper>
            );
          //if (id === 3) return <Watch key={3} />;
          // if (id === 2) return <MockWidget2 key={2} />;
          // if (id === 3) return <MockWidget3 key={3} />;
        })}
      </Box>
    </>
  );
}

export default MirrorDashboard;
