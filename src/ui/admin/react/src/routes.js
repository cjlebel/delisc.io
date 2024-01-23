/*!

=========================================================
* Black Dashboard React v1.2.2
=========================================================

* Product Page: https://www.creative-tim.com/product/black-dashboard-react
* Copyright 2023 Creative Tim (https://www.creative-tim.com)
* Licensed under MIT (https://github.com/creativetimofficial/black-dashboard-react/blob/master/LICENSE.md)

* Coded by Creative Tim

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
import Dashboard from "views/Dashboard.js";
import Icons from "views/Icons.js";
import LinkDetails from "views/LinkDetails";
import Links from "views/Links.jsx";
import Map from "views/Map.js";
import Notifications from "views/Notifications.js";
import Settings from "views/Settings";
import TableList from "views/TableList.js";
import Typography from "views/Typography.js";
import UserProfile from "views/UserProfile.js";
import Users from "views/Users.jsx";

var routes = [
    {
        path: "/dashboard",
        name: "Dashboard",
        icon: "tim-icons icon-chart-pie-36",
        component: <Dashboard />,
        layout: "/admin",
    },
    {
        path: "/links",
        name: "Links",
        icon: "tim-icons icon-link-72",
        component: <Links />,
        layout: "/admin",
    },
    {
        path: "/links/:id",
        component: <LinkDetails />,
        layout: "/admin",
    },
    {
        path: "/tags",
        name: "Tags",
        icon: "tim-icons icon-tag",
        component: <LinkDetails />,
        layout: "/admin",
    },
    {
        path: "/users",
        name: "Users",
        icon: "tim-icons icon-single-02",
        component: <Users />,
        layout: "/admin",
    },
    {
        path: "/settings",
        name: "Settings",
        icon: "tim-icons icon-components",
        component: <Settings />,
        layout: "/admin",
    },

    {
        path: "/users/:id",
        component: <UserProfile />,
        layout: "/admin",
    },
    {
        path: "/icons",
        name: "Icons",
        icon: "tim-icons icon-atom",
        component: <Icons />,
        layout: "/admin",
    },
    {
        path: "/map",
        name: "Map",
        icon: "tim-icons icon-pin",
        component: <Map />,
        layout: "/admin",
    },
    {
        path: "/notifications",
        name: "Notifications",
        icon: "tim-icons icon-bell-55",
        component: <Notifications />,
        layout: "/admin",
    },
    {
        path: "/user-profile",
        name: "User Profile",
        icon: "tim-icons icon-single-02",
        component: <UserProfile />,
        layout: "/admin",
    },
    {
        path: "/tables",
        name: "Table List",
        icon: "tim-icons icon-puzzle-10",
        component: <TableList />,
        layout: "/admin",
    },
    {
        path: "/typography",
        name: "Typography",
        icon: "tim-icons icon-align-center",
        component: <Typography />,
        layout: "/admin",
    },
];
export default routes;
