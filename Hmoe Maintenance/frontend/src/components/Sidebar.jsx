import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { FaHome, FaTools, FaUserTie, FaBuilding, FaClipboardList } from 'react-icons/fa';

const Sidebar = () => {
  const { user } = useContext(AuthContext);

  if (!user) return null;

  const roles = user.roles || [];

  return (
    <aside className="w-64 bg-gray-800 text-white min-h-screen p-4">
      <ul className="space-y-4">
        <li>
          <Link to="/" className="flex items-center space-x-2 hover:text-gray-300">
            <FaHome />
            <span>Home</span>
          </Link>
        </li>
        
        {roles.includes('Admin') && (
          <>
            <li className="pt-4 pb-2 text-xs text-gray-400 uppercase">Admin Panel</li>
            <li>
              <Link to="/admin/companies" className="flex items-center space-x-2 hover:text-gray-300">
                <FaBuilding />
                <span>Manage Companies</span>
              </Link>
            </li>
            <li>
              <Link to="/admin/technicians" className="flex items-center space-x-2 hover:text-gray-300">
                <FaUserTie />
                <span>Manage Technicians</span>
              </Link>
            </li>
          </>
        )}

        {roles.includes('CompanyOwner') && (
          <>
            <li className="pt-4 pb-2 text-xs text-gray-400 uppercase">Company Dashboard</li>
            <li>
              <Link to="/company/profile" className="flex items-center space-x-2 hover:text-gray-300">
                <FaBuilding />
                <span>My Profile</span>
              </Link>
            </li>
            <li>
              <Link to="/company/requests" className="flex items-center space-x-2 hover:text-gray-300">
                <FaClipboardList />
                <span>Maintenance Requests</span>
              </Link>
            </li>
          </>
        )}

        {roles.includes('Technical') && (
          <>
            <li className="pt-4 pb-2 text-xs text-gray-400 uppercase">Technician Dashboard</li>
            <li>
              <Link to="/technical/jobs" className="flex items-center space-x-2 hover:text-gray-300">
                <FaTools />
                <span>My Jobs</span>
              </Link>
            </li>
          </>
        )}

        {roles.includes('Client') && (
          <>
            <li className="pt-4 pb-2 text-xs text-gray-400 uppercase">Client Area</li>
            <li>
              <Link to="/client/requests" className="flex items-center space-x-2 hover:text-gray-300">
                <FaClipboardList />
                <span>My Requests</span>
              </Link>
            </li>
          </>
        )}
      </ul>
    </aside>
  );
};

export default Sidebar;
