import { Box } from "@mui/material";
import { useDrag } from "react-dnd";

function MockWidget1() {
  const [{ isDragging }, drag] = useDrag(() => ({
    type: "widget",
    item: { id: 1 },
    collect: (monitor) => ({
      isDragging: monitor.isDragging(),
    }),
  }));

  return (
    <div ref={drag as unknown as React.RefObject<HTMLDivElement>}>
      <Box
        sx={{
          width: "200px",
          height: "150px",
          border: `5px solid blue`,
          borderRadius: "16px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          margin: "5px",
          opacity: isDragging ? 0.5 : 1,
          cursor: "grab",
        }}
      >
        <h2>Widget 1</h2>
      </Box>
    </div>
  );
}

export default MockWidget1;
