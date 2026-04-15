import { Box } from "@mui/material";
import { useState } from "react";
import { useDrop } from "react-dnd";
import DraggableWrapper from "./DraggableWrapper";
import WeatherWidget from "./WeatherWidget";
import Watch from "../base/watch";
import FlightInfo from "./FlightInfoWidget";
import ArrivalsWidget from "./ArrivalFlightsWidget.tsx";
import DeparturesWidget from "./DepartureFlightsWidget.tsx";
import { widgetTheme } from "../../theme/index.ts";

function MirrorDashboard() {
  const [order, setOrder] = useState([1, 2, 3, 4, 5]);

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
            padding: 0,
            backgroundColor: `${widgetTheme.palette.primary.dark}`,
          }}
        >
          <Box
            ref={drop as unknown as React.RefObject<HTMLDivElement>}
            sx={{
              border: `25px solid white`,
              boxShadow: `inset 0 0 0 4px ${widgetTheme.palette.primary.light}`,
              outlineOffset: "-24px",
              paddingRight: { xs: "1rem", sm: "3rem", md: "10rem" },
              backgroundColor: isOver ? "rgba(0,0,0,0.1)" : "transparent",
              display: "flex",
              flexWrap: "wrap",
              alignContent: "flex-start",
              paddingBottom: { xs: "10rem", sm: "20rem", md: "30rem" },
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
                    {/*För att se de olika layouterna för pilot eller "vamlig" gäst byt boolen nedan. (false = vanlig gäst) */}
                    <WeatherWidget icao="ESSA" pilotVersion={false} />
                  </DraggableWrapper>
                );
              if (id === 3)
                return (
                  <DraggableWrapper key={3} id={3}>
                    <FlightInfo airport="ARN" flight="OS966" />
                  </DraggableWrapper>
                );
              if (id === 4)
                return (
                  <DraggableWrapper key={4} id={4}>
                    <ArrivalsWidget />
                  </DraggableWrapper>
                );
              if (id === 5)
                return (
                  <DraggableWrapper key={5} id={5}>
                    <DeparturesWidget />
                  </DraggableWrapper>
                );
            })}
          </Box>
        </Box>
      </Box>
    </>
  );
}

export default MirrorDashboard;
