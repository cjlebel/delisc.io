import React, { useState } from 'react';

import { API_URL } from '@/utils/Configs';

const API_KEY = process.env.REACT_APP_API_KEY;
const USER_AGENT = 'deliscio-web-client';

export default function Login({}) {
   const [formData, setFormData] = useState({
      email: '',
      password: '',
   });

   const [error, setError] = useState<string | null>(null);

   const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;
      setFormData({ ...formData, [name]: value });
   };

   const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
      e.preventDefault();

      try {
         const signinRequest = {
            EmailOrUserName: formData.email,
            Password: formData.password,
         };

         let response = await fetch(`${API_URL}/auth/signin`, {
            method: 'POST',
            headers: {
               'x-api-key': `${API_KEY}`,
               'User-Agent': `${USER_AGENT}`,
            },
            body: JSON.stringify(signinRequest),
         })
            .then((res) => res.json())
            .catch((error) => {
               console.error(error);
            });

         // Handle successful authentication (e.g., redirect to dashboard)
         console.log('Authentication successful!', response.data);
      } catch (err) {
         // Handle authentication error
         setError('Invalid email or password. Please try again.');
         console.error('Authentication error:', err);
      }
   };

   return (
      <div className='container mt-5'>
         <h1 className='mb-4'>Login</h1>
         {error && <div className='alert alert-danger'>{error}</div>}
         <form onSubmit={handleSubmit}>
            <div className='mb-3'>
               <label htmlFor='email' className='form-label'>
                  Email address
               </label>
               <input
                  type='email'
                  className='form-control'
                  id='email'
                  name='email'
                  value={formData.email}
                  onChange={handleChange}
                  required
               />
            </div>
            <div className='mb-3'>
               <label htmlFor='password' className='form-label'>
                  Password
               </label>
               <input
                  type='password'
                  className='form-control'
                  id='password'
                  name='password'
                  value={formData.password}
                  onChange={handleChange}
                  required
               />
            </div>
            <button type='submit' className='btn btn-primary'>
               Login
            </button>
         </form>
      </div>
   );
}
