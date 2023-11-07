import React from 'react';
import { createRoot } from 'react-dom/client';

import Counter from './counter';

const domNode = document.getElementById('counter-container');

if (domNode) {
    const root = createRoot(domNode);
    root.render(<React.StrictMode><Counter /></React.StrictMode>);
}

