import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import MirrorDashboard from "./MirrorDashboard";

function MirrorDndProvider() {
  return (
    <DndProvider backend={HTML5Backend}>
      <MirrorDashboard />
    </DndProvider>
  );
}

export default MirrorDndProvider;
