import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { notification } from "antd";

export default function Register() {
    const [userName, setUserName] = useState('');
    const [userPassword, setUserPassword] = useState('');
    const [userEmail, setUserEmail] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            const res = await fetch('/api/auth/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email: userEmail, username: userName, password: userPassword })
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            notification.success({ message: `${userName} successfully registered!` });
            navigate('/');
        }
        catch (error) {
            notification.error({ message: `${userName} already exist!` });
        }
    }

    return (
        <>
            <form className='register' onSubmit={handleSubmit}>
                <label>Username</label>
                <input type='text' onChange={e => setUserName(e.target.value)} value={userName} required />
                <label>Password</label>
                <input type='password' onChange={e => setUserPassword(e.target.value)} value={userPassword} required />
                <label>Email</label>
                <input type='email' onChange={e => setUserEmail(e.target.value)} value={userEmail} required />
                <button type='submit'>Register</button>
            </form>
        </>
    );
}