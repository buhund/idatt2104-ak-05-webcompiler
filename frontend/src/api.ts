// src/api.js
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5240/api';

export const compileCode = async (code) => {
  try {
    const response = await axios.post(`${API_BASE_URL}/compilation/compile`, { code });
    return response.data;
  } catch (error) {
    console.error('Error during compile API call:', error);
    throw error; // Throwing error to handle it in the calling component
  }
};
