import './App.css'
import { Link } from 'react-router';


function App() {
  

  return (
    <>
    <h1>DashyBoard</h1>
      
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
  )
}

export default App
