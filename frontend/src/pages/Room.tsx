import { useParams } from 'react-router';

export default function Room() {
    const roomNr = useParams();

    return (<>
    <h1>Room { roomNr.id }</h1>
    </>);
}