import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { notification } from 'antd';

notification.config({
  duration: 2,
  closeIcon: null
})

export default function Login({ setLoggedUser }) {

  const [userName, setUserName] = useState('');
  const [userPassword, setUserPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async e => {
    e.preventDefault();
    try {
      const res = await fetch(`api/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName: userName, password: userPassword })
      });
      if (!res.ok) {
        throw new Error(`HTTP error! status: ${res.status}`);
      }
      const loggedUser = await res.json();
      setLoggedUser(loggedUser);
      notification.success({ message: `${loggedUser.userName} logged in!` });
      navigate('/');
    }
    catch (error) {
      notification.error({ message: 'User or password incorrect!' });
    }
  }

  return (
    <>
      <form className='login' onSubmit={e => handleLogin(e)}>
        <label>Username</label>
        <input type='text' onChange={e => setUserName(e.target.value)} value={userName} required />
        <label>Password</label>
        <input type='password' onChange={e => setUserPassword(e.target.value)} value={userPassword} required />
        <button type='submit'>Login</button>
      </form>
    </>
  );
}