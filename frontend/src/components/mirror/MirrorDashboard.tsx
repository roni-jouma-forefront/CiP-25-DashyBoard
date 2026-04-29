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
import GuestName from "./GuestName.tsx";
import WeatherWidgetDestination from "./WeatherWidgetDestination.tsx";
import { widgetTheme } from "../../theme/index.ts";
import { useBookings } from "../../hooks";
import { useParams } from "react-router";

function MirrorDashboard() {
  const [order, setOrder] = useState([1, 2, 3, 4, 5, 6, 7, 8]);

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

  const { bookingId } = useParams();
  const { data, error, isLoading } = useBookings({
    bookingId: bookingId as string,
  });

  if (!data) {
    return <div>Ingen data</div>;
  }
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
          color: `${widgetTheme.palette.primary.light}`,
        }}
      >
        Loading bookings info...
      </Typography>
    );

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
          <GuestName guestId={data.guestId} />
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
                    <WeatherWidget
                      icao={import.meta.env.VITE_AIRPORT_ICAO}
                      pilotVersion={true}
                    />
                  </DraggableWrapper>
                );
              if (id === 3 && data.flightNumber)
                return (
                  <DraggableWrapper key={3} id={3}>
                    <FlightInfo
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                      flight={data.flightNumber}
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
              if (id === 7)
                return (
                  <DraggableWrapper key={7} id={7}>
                    <WaitTimeWidget
                      airport={import.meta.env.VITE_AIRPORT_NAME}
                    />
                  </DraggableWrapper>
                );
              if (id === 8)
                return (
                  <DraggableWrapper key={8} id={8}>
                    <WeatherWidgetDestination icao="LOWW" pilotVersion={true} />
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
