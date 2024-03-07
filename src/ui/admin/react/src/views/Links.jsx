import React from "react";

import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
} from "reactstrap";

export default function Links() {
    return (
        <>
            <div className="content">
                <h1>Links</h1>
                <Row>
                    <Col md="12">
                        <Card>
                            <CardHeader>
                                <CardTitle tag="h4">Simple Table</CardTitle>
                            </CardHeader>
                            <CardBody>
                                <Table className="tablesorter" responsive>
                                    <thead className="text-primary">
                                        <tr>
                                            <th>Title</th>
                                            <th>Status</th>
                                            <th className="text-center">
                                                Date
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <a href="/admin/links/3456345">
                                                    Dakota Rice
                                                </a>
                                            </td>
                                            <td>Niger</td>
                                            <td>Oud-Turnhout</td>
                                        </tr>
                                        <tr>
                                            <td>Minerva Hooper</td>
                                            <td>Curaçao</td>
                                            <td>Sinaai-Waas</td>
                                        </tr>
                                        <tr>
                                            <td>Sage Rodriguez</td>
                                            <td>Netherlands</td>
                                            <td>Baileux</td>
                                        </tr>
                                        <tr>
                                            <td>Philip Chaney</td>
                                            <td>Korea, South</td>
                                            <td>Overland Park</td>
                                        </tr>
                                        <tr>
                                            <td>Doris Greene</td>
                                            <td>Malawi</td>
                                            <td>Feldkirchen in Kärnten</td>
                                            <td className="text-center">
                                                $63,542
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Mason Porter</td>
                                            <td>Chile</td>
                                            <td>Gloucester</td>
                                            <td className="text-center">
                                                $78,615
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Jon Porter</td>
                                            <td>Portugal</td>
                                            <td>Gloucester</td>
                                            <td className="text-center">
                                                $98,615
                                            </td>
                                        </tr>
                                    </tbody>
                                </Table>
                            </CardBody>
                        </Card>
                    </Col>
                </Row>
            </div>
        </>
    );
}
