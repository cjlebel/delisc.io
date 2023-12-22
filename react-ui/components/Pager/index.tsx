import React from "react";

type Props = {
    pageNo: number;
};

export default function Pager({ pageNo }: Props) {
    return <div>{pageNo}</div>;
}
