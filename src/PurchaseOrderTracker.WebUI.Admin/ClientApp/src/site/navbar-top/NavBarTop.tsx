import React from 'react';
import './NavBarTop.scss';
import logo from './logo.png';
import { Navbar, NavbarBrand, NavbarText } from 'reactstrap';

// todo
// move urls into constants
// build logout page
const NavBarTop: React.FC = () => {
  return (
    <Navbar className="bg-dark text-white">
      <NavbarBrand>
        <a href="/main-site">
          <img src={logo} alt="logo" className="mr-2" />
        </a>
        Purchase Order Tracker - Administration
      </NavbarBrand>
      <NavbarText tag="a" href="/logout" className="text-white">
        Logout
      </NavbarText>
    </Navbar>
  );
};

export default NavBarTop;
