import "./App.css";
import { Link } from "react-router";
import Watch from "./components/base/watch";

function App() {
  return (
    <>
      <h1>DashyBoard</h1>
      <Watch location="Stockholm" timeZone="UTC"></Watch>

      <nav>
        <ul>
          <li>
            <Link to="/">Admin</Link>
          </li>
          <li>
            <Link to="/room/123">Room -default</Link>
          </li>
        </ul>
      </nav>
    </>
  );
}

export default App;
