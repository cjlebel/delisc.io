import React from 'react'

type Props = {}

export default function Counter({ }: Props) {
    const [currentCount, setCurrentCount] = React.useState(0);

    function incrementCounter() {
        setCurrentCount((prev: number) => prev + 1);
    };
    return (
        <div className="card">
            <h1>Counter</h1>
            <p>Current count: <strong>{currentCount}</strong></p>
            <button onClick={() => incrementCounter()}>Increment
            </button>
        </div>
    )
}
