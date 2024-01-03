import { API_URL } from '@/utils/Configs';

const API_KEY = process.env.REACT_APP_API_KEY;
const USER_AGENT = 'deliscio-web-client';

const apiUserLogin = async (username: string, password: string) => {
   if (!username || !password) throw new Error('Username and password are required');

   const signinRequest = {
      EmailOrUserName: username,
      Password: password,
   };

   let data = await fetch(`${API_URL}/auth/signin`, {
      method: 'POST',
      headers: {
         'x-api-key': `${API_KEY}`,
         'User-Agent': `${USER_AGENT}`,
         //Accept: '*/*',
      },
      body: JSON.stringify(signinRequest),
   })
      .then((res) => res.json())
      .then((data) => {
         console.log(data);
      })
      .catch((error) => {
         console.error(error);
      });
};

export { apiUserLogin };
