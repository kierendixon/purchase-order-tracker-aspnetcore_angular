import React from 'react';

import { Nav, NavItem, NavLink, Navbar, NavbarBrand } from 'reactstrap';
import { logoutUrl, mainSiteUrl } from '../../config/routing.config';

import logo from './logo.png';

const NavBarTop = () => {
  return (
    <Navbar className="bg-dark text-white" expand>
      <NavbarBrand>
        <img src={logo} alt="logo" width={30} height={30} className="ml-2 align-top" />
        &nbsp;Purchase Order Tracker - Administration
      </NavbarBrand>
      <Nav className="ml-auto" navbar>
        <NavItem>
          <NavLink href={mainSiteUrl} className="text-white">
            Main Site
          </NavLink>
        </NavItem>
        <NavItem>
          <NavLink href={logoutUrl} className="text-white">
            Logout
          </NavLink>
        </NavItem>
      </Nav>
    </Navbar>
  );
};

export default NavBarTop;
