import React from 'react';
import { useNavigate } from 'react-router-dom';


const Navbar: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    navigate('/login');
  };

  return (
    <nav>
      <ul className='flex items-center justify-between'>
        <li><a href="">Home</a></li>
        <li><a href="">Portfolio</a></li>
        <li><a href="">About me</a></li>
        <li><a href="">Contact</a></li>
      </ul>
    </nav>

  );
};

export default Navbar;
