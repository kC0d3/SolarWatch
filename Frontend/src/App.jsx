import { BrowserRouter as Router, Routes, Route} from 'react-router-dom';
import { useState } from 'react';
import NavBar from './Components/NavBar.jsx';
import Home from './Components/Home.jsx';
import Register from './Components/Register.jsx';
import Login from './Components/Login.jsx';
import ErrorPage from './Components/ErrorPage.jsx';

function App() {
  const [loggedUser, setLoggedUser] = useState(undefined);
  
  return (
    <Router>
      <Routes>
        <Route path='/' element={<NavBar {...{ loggedUser, setLoggedUser }} />}>
          <Route index element={<Home {...{loggedUser}}/>} />
          <Route path='register' element={<Register />}></Route>
          <Route path='login' element={<Login {...{ setLoggedUser }} />}></Route>
        </Route>
        <Route path='*' element={<ErrorPage />} />
      </Routes>
    </Router>
  );
}

export default App;