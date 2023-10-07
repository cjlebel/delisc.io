import React from 'react';
import styles from './Navigation.module.scss';

type FooterProps = {};

const Footer = (props: FooterProps) => {
    return (
        <footer className={styles.footer}>
            <p>
                <span className='deliscio'>Delisc.io</span>
            </p>
        </footer>
    );
};

export default Footer;
