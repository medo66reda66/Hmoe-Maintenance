import api from './axiosConfig';

export const authService = {
  login: async (credentials) => {
    const response = await api.post('/Auth/Login', credentials);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('refreshToken', response.data.refreshtoken);
    }
    return response.data;
  },

  register: async (data, addressData) => {
    // API uses query string for address Request based on the controller `[FromQuery] AddressRequest addressRequest`
    // Convert address data to query parameters
    const params = new URLSearchParams(addressData).toString();
    const response = await api.post(`/Auth/Register?${params}`, data);
    return response.data;
  },

  logout: async () => {
    try {
      await api.post('/Auth/Logout');
    } catch (error) {
      console.error('Logout error', error);
    } finally {
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
    }
  },

  forgetPassword: async (email) => {
    const response = await api.post('/Auth/ForgetPassword', { email });
    return response.data;
  }
};
