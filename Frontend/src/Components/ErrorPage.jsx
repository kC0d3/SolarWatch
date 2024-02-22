import { useNavigate } from "react-router-dom";

export default function ErrorPage() {
  const navigate = useNavigate();
  return (
    <div className="page">
      <h1 style={{ color: "red" }}>404</h1>
      <h2 style={{ color: "red" }}>Page does not exist</h2>
      <button onClick={() => navigate('/')}>Home</button>
    </div>
  );
}
