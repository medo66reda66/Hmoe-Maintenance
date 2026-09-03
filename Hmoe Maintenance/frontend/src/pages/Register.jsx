import React, { useState } from 'react';
import { authService } from '../api/authService';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    roleId: 2, // Default to Client or something, needs mapping
    phone: ''
  });

  const [addressData, setAddressData] = useState({
    Governorate: '',
    City: '',
    Street: ''
  });

  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });
  const handleAddressChange = (e) => setAddressData({ ...addressData, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      setError("Passwords don't match");
      return;
    }
    try {
      await authService.register(formData, addressData);
      navigate('/login');
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed');
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100 py-10">
      <div className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-4 text-center">Register</h2>
        {error && <p className="text-red-500 mb-4">{error}</p>}
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-gray-700 mb-2 text-sm">First Name</label>
              <input type="text" name="firstName" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
            </div>
            <div>
              <label className="block text-gray-700 mb-2 text-sm">Last Name</label>
              <input type="text" name="lastName" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
            </div>
          </div>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">User Name</label>
            <input type="text" name="userName" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
          </div>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">Email</label>
            <input type="email" name="email" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
          </div>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">Phone</label>
            <input type="text" name="phone" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-gray-700 mb-2 text-sm">Password</label>
              <input type="password" name="password" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
            </div>
            <div>
              <label className="block text-gray-700 mb-2 text-sm">Confirm Password</label>
              <input type="password" name="confirmPassword" className="w-full border p-2 rounded text-sm" onChange={handleChange} required />
            </div>
          </div>

          <h3 className="font-semibold text-gray-700 mt-4 border-b pb-2">Address Info</h3>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">Governorate</label>
            <input type="text" name="Governorate" className="w-full border p-2 rounded text-sm" onChange={handleAddressChange} required />
          </div>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">City</label>
            <input type="text" name="City" className="w-full border p-2 rounded text-sm" onChange={handleAddressChange} required />
          </div>
          <div>
            <label className="block text-gray-700 mb-2 text-sm">Street</label>
            <input type="text" name="Street" className="w-full border p-2 rounded text-sm" onChange={handleAddressChange} required />
          </div>
          
          <div>
             <label className="block text-gray-700 mb-2 text-sm">Role</label>
             <select name="roleId" className="w-full border p-2 rounded text-sm" onChange={handleChange}>
               <option value="4">Client</option>
               <option value="2">Company Owner</option>
               <option value="3">Technical</option>
             </select>
          </div>

          <button type="submit" className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 mt-4">
            Register
          </button>
        </form>
      </div>
    </div>
  );
};

export default Register;
