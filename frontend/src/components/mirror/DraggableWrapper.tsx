import { useDrag } from "react-dnd";

interface Props {
  id: number;
  children: React.ReactNode;
}

export default function DraggableWrapper({ id, children }: Props) {
  const [{ isDragging }, drag] = useDrag(() => ({
    type: "widget",
    item: { id },
    collect: (monitor) => ({
      isDragging: monitor.isDragging(),
    }),
  }));

  return (
    <div
      ref={drag as unknown as React.RefObject<HTMLDivElement>}
      style={{ opacity: isDragging ? 0.5 : 1, cursor: "grab" }}
    >
      {children}
    </div>
  );
}
