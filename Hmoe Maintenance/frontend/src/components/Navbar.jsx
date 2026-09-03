import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { FaUserCircle, FaSignOutAlt } from 'react-icons/fa';

const Navbar = () => {
  const { user, logout } = useContext(AuthContext);

  return (
    <nav className="bg-blue-600 text-white shadow-md p-4">
      <div className="container mx-auto flex justify-between items-center">
        <Link to="/" className="text-xl font-bold">Home Maintenance</Link>
        <div className="flex items-center space-x-4">
          {user ? (
            <>
              <div className="flex items-center space-x-2">
                <FaUserCircle className="text-2xl" />
                <span>{user.email}</span>
                <span className="text-xs bg-blue-800 px-2 py-1 rounded">
                  {user.roles.join(', ')}
                </span>
              </div>
              <button 
                onClick={logout}
                className="flex items-center space-x-1 bg-red-500 hover:bg-red-600 px-3 py-1 rounded transition"
              >
                <FaSignOutAlt />
                <span>Logout</span>
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="hover:underline">Login</Link>
              <Link to="/register" className="bg-white text-blue-600 px-3 py-1 rounded hover:bg-gray-100 transition">Register</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
