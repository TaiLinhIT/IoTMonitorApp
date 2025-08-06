import React from 'react';
import Navbar from '../components/Navbar';

const Dashboard: React.FC = () => {
  const username = localStorage.getItem('username'); // hoặc lấy từ API

  return (
    <div className="min-h-screen bg-gray-100">
      <Navbar />
      <div className="p-6">
        <h1 className="text-3xl font-bold mb-4">Welcome to the Dashboard</h1>
        <p className="text-lg">Hello, {username || 'User'}! You are logged in.</p>
      </div>
    </div>
  );
};

export default Dashboard;
