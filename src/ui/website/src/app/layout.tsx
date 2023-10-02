import 'bootstrap/dist/css/bootstrap.min.css';
import '@/styles/globals.scss';

import { Inter } from 'next/font/google';

import { Header, Footer } from '@/components/elements/navigation';
import BreadCrumbs from '@/components/elements/breadcrumbs';

const inter = Inter({ subsets: ['latin'] });

export const metadata = {
   title: 'Delisc.io',
   description: 'A delicious clone',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
   return (
      <html lang='en'>
         <body className={`${inter.className} d-flex flex-column`}>
            <Header />
            <BreadCrumbs />
            <main className='container'>{children}</main>
            <Footer />
         </body>
      </html>
   );
}
