import { useParams } from "react-router";
import { RoomDetailsForm } from "../../components/admin/forms/RoomDetailsForm";

export default function Room() {
  const { id } = useParams();

  return (
    <>
      <h1>Details for room {id}</h1>
      <RoomDetailsForm />
    </>
  );
}
