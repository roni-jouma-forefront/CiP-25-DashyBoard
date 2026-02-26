import { Box } from "@mui/material";
import { useState } from "react";
import { useDrop } from "react-dnd";
import MockWidget1 from "./MockWidget1";

function MirrorDashboard() {
  const [order, setOrder] = useState([1, 2, 3]);

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
      <h1>Spegelsida</h1>
      <Box
        ref={drop as unknown as React.RefObject<HTMLDivElement>}
        sx={{
          border: "30px solid",
          borderImage: "linear-gradient(45deg, #f06, #09f) 1",
          padding: { xs: "1rem", sm: "3rem", md: "10rem" },
          backgroundColor: isOver ? "rgba(0,0,0,0.1)" : "transparent",
          display: "flex",
          justifyContent: "center",
        }}
      >
        {order.map((id) => {
          if (id === 1) return <MockWidget1 key={1} />;
          // if (id === 2) return <MockWidget2 key={2} />;
          // if (id === 3) return <MockWidget3 key={3} />;
        })}
      </Box>
    </>
  );
}

export default MirrorDashboard;
