import logo from './logo.png';
import React from 'react';
import { Navbar, NavbarBrand, NavbarText } from 'reactstrap';
import { logoutUrl, mainSiteUrl } from '../config/routing.config';

const NavBarTop: React.FC = () => {
  return (
    <Navbar className="bg-dark text-white">
      <NavbarBrand>
        <a href={mainSiteUrl}>
          <img src={logo} alt="logo" width={30} height={30} className="ml-2 align-top" />
        </a>
        &nbsp;Purchase Order Tracker - Administration
      </NavbarBrand>
      <NavbarText tag="a" href={logoutUrl} className="text-white">
        Logout
      </NavbarText>
    </Navbar>
  );
};

export default NavBarTop;
