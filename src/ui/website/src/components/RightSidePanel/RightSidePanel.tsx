import React, { ReactNode } from 'react';

import styles from './RightSidePanel.module.scss';

type RightSidePanelProps = {
   title?: string | null;
   children?: ReactNode | null;
};

export default function RightSidePanel({ title, children }: RightSidePanelProps) {
   return (
      <div className={styles.panel}>
         <h4 className={`${styles['title']} title`}>{title}</h4>
         <div className={`${styles['contents']} contents`}>{children}</div>
      </div>
   );
}
