import React, { useState } from 'react';
import './forgotPassword.css'; 
import { useNavigate } from 'react-router-dom';

const ForgotPassword = () => {
  const [email, setEmail] = useState('');
  const [confirmEmail, setConfirmEmail] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleResetPassword = (e) => {
    e.preventDefault();
    if (email !== confirmEmail) {
      setError('Los correos electrónicos no coinciden.');
      return;
    }


    alert('Se ha enviado un correo para restablecer tu contraseña.');
    navigate('/login'); 
  };

  return (
    <div className="forgot-password-container">
      <h2>Restablecer Contraseña</h2>
      <form onSubmit={handleResetPassword}>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Introduce tu correo electrónico"
          required
        />
        <input
          type="email"
          value={confirmEmail}
          onChange={(e) => setConfirmEmail(e.target.value)}
          placeholder="Confirma tu correo electrónico"
          required
        />
        <button type="submit">Enviar correo de restablecimiento</button>
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  );
};

export default ForgotPassword;
