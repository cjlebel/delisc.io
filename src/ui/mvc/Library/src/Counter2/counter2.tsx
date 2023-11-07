import React from 'react'

type Props = {}

export default function Counter2({ }: Props) {
    const [currentCount, setCurrentCount] = React.useState(0);

    function incrementCounter() {
        setCurrentCount((prev: number) => prev + 1);
    };
    return (
        <div>
            <h1>Counter 2</h1>
            <p>Current count: <strong>{currentCount}</strong></p>
            <button onClick={() => incrementCounter()}>Increment
            </button>
        </div>
    )
}