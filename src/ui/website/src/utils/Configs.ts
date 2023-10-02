import dotenv from 'dotenv';

// Load environment variables from the appropriate .env file based on NODE_ENV
const envFile = process.env.NODE_ENV === 'production' ? '.env.production' : '.env.development';

dotenv.config({ path: envFile });

var api_url = process.env.REACT_APP_API_URL;
var api_version = process.env.REACT_APP_API_VERSION;

export const API_URL = `${api_url}/${api_version}`;
