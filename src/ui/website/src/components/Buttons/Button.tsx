import React from 'react';

type ButtonProps = {
   className?: string | '';
   text: string;
};

export default function Button(props: ButtonProps) {
   return <button className={props.className}>{props.text}</button>;
}
