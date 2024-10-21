import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './register.css';

const Register = ({ darkMode }) => {
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false); 
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleRegister = async (e) => {
    e.preventDefault();
    setLoading(true); 
    setError(''); 

    try {
      const response = await fetch('/api/user/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          nombre: name, 
          apellido: surname, 
          correoElectronico: email, 
          contrasenaHash: password, 
        }),
      });

      const data = await response.json();

      if (response.ok) {
        navigate('/login'); 
      } else {
        setError(data.message || 'Error en el registro'); 
      }
    } catch (error) {
      setError('Error en la conexión'); 
    } finally {
      setLoading(false); 
    }
  };

  return (
    <div className={`register-container ${darkMode ? 'dark' : ''}`}>
      <h2>Registro</h2>
      <form onSubmit={handleRegister}>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Nombre"
          required
        />
        <input
          type="text"
          value={surname}
          onChange={(e) => setSurname(e.target.value)}
          placeholder="Apellidos"
          required
        />
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Correo electrónico"
          required
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Contraseña"
          required
        />
        <button type="submit" disabled={loading}>
          {loading ? 'Registrando...' : 'Registrarse'} 
        </button>
        {error && <p className="error">{error}</p>} 
      </form>

      <p>
        ¿Ya tienes cuenta?{' '}
        <span onClick={() => navigate('/login')} className="login-link">
          Inicia sesión aquí
        </span>
      </p>
    </div>
  );
};

export default Register;
