import { Box, Typography } from "@mui/material";
import { useState } from "react";
import { useDrop } from "react-dnd";
import DraggableWrapper from "./DraggableWrapper";
import WeatherWidget from "./WeatherWidget";
import Watch from "../base/watch";
import FlightInfo from "./FlightInfoWidget";
import ArrivalsWidget from "./ArrivalFlightsWidget.tsx";
import DeparturesWidget from "./DepartureFlightsWidget.tsx";
import MessagesWidget from "./MessagesWidget.tsx";
import WaitTimeWidget from "./WaitTimesWidget.tsx";
import { widgetTheme } from "../../theme/index.ts";
import { useBookings } from "../../hooks";
import { useParams } from "react-router";

function MirrorDashboard() {
  const [order, setOrder] = useState([1, 2, 3, 4, 5, 6]);
  const guestName = "Hozier";

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

  const { roomId } = useParams();

  const {
    data: bookings = {},
    error,
    isLoading,
  } = useBookings({
    roomId,
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

  console.log(bookings);

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
            sx={{
              display: "flex",
              justifyContent: "center",
              m: 2,
            }}
          >
            <Typography
              variant="h3"
              sx={{ color: `${widgetTheme.palette.primary.main}` }}
            >
              Welcome {guestName}
            </Typography>
          </Box>
          <Box
            ref={drop as unknown as React.RefObject<HTMLDivElement>}
            sx={{
              border: `25px solid ${widgetTheme.palette.primary.light}`,
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
                      location={import.meta.env.VITE_LOCATION_NAME}
                      timeZone={import.meta.env.VITE_TIMEZONE}
                    />
                  </DraggableWrapper>
                );
              if (id === 2)
                return (
                  <DraggableWrapper key={2} id={2}>
                    {/*För att se de olika layouterna för pilot eller "vanlig" gäst byt boolen nedan. (false = vanlig gäst) */}
                    <WeatherWidget icao="ESSA" pilotVersion={false} />
                  </DraggableWrapper>
                );
              if (id === 3)
                return (
                  <DraggableWrapper key={3} id={3}>
                    <FlightInfo
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                      flight="OS966"
                    />
                  </DraggableWrapper>
                );
              if (id === 4)
                return (
                  <DraggableWrapper key={4} id={4}>
                    <ArrivalsWidget
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                      timezone={import.meta.env.VITE_TIMEZONE}
                    />
                  </DraggableWrapper>
                );
              if (id === 5)
                return (
                  <DraggableWrapper key={5} id={5}>
                    <DeparturesWidget
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                    />
                  </DraggableWrapper>
                );
              if (id === 6)
                return (
                  <DraggableWrapper key={6} id={6}>
                    <MessagesWidget
                      hotelId={import.meta.env.VITE_HOTEL_ID}
                      roomId={"20000000-0000-0000-0000-000000000101"}
                    />
                  </DraggableWrapper>
                );
              if (id === 6)
                return (
                  <DraggableWrapper key={6} id={6}>
                    <WaitTimeWidget
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                    />
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
