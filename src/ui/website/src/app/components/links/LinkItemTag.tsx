import React from "react";

export type LinkItemTagProps = {
    id: string;
    name: string;
    count: number;
    weight: number;
};

export default function LinkItemTag(tag: LinkItemTagProps) {
    return <div>{tag.name}</div>;
}
