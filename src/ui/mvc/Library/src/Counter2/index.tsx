import React from 'react';
import { createRoot } from 'react-dom/client';

import Counter2 from './counter2';

const counter2Node = document.getElementById('counter2-container');

if (counter2Node) {
    const root = createRoot(counter2Node);
    root.render(<React.StrictMode><Counter2 /></React.StrictMode>);
}

