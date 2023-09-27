import React from "react";
import LinkItemTag from "./LinkItemTag";
import { LinkItemTagProps } from "./LinkItemTag";

type LinkItemProps = {
    title: string;
    url: string;
    description: string;
    tags: LinkItemTagProps[];
};

export default function LinkItem(props: LinkItemProps) {
    const tagItems = props.tags.map((tag) => (
        <LinkItemTag key={tag.name} {...tag} />
    ));

    return (
        <div>
            <h3>{props.title}</h3>
            <p>{props.description}</p>
            <p>{tagItems}</p>
        </div>
    );
}
