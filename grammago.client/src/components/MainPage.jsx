import React from 'react';

const MainPage = ({ darkMode }) => {
  return (
    <div className={`main-page-container ${darkMode ? 'dark' : ''}`}>
      <h2>Bienvenido a GrammaGO!</h2>
      <p>Aquí podrás acceder a tus lecciones de gramática.</p>
      {/* Aquí añadire la API que vaya a usar de ejercicio de inglés */}
    </div>
  );
};

export default MainPage;
