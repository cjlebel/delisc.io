import React from 'react';
import styles from './Navigation.module.scss';
type HeaderProps = {};

const Header = (props: HeaderProps) => {
   return (
      <header className={`${styles.header} container`}>
         <nav>
            <ul>
               <li>
                  <a href='/'>Home</a>
               </li>
               <li>
                  <a href='/bookmarks'>Bookmarks</a>
               </li>
               <li>
                  <a href='/tags'>Tags</a>
               </li>
               <li>
                  <a href='/profile'>Profile</a>
               </li>
            </ul>
         </nav>
      </header>
   );
};

export default Header;
