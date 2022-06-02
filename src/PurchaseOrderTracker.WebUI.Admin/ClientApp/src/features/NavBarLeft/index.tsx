import './NavBarLeft.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Nav, NavItem, NavLink } from 'reactstrap';
import React, { useState } from 'react';
import { faBuilding, faUser } from '@fortawesome/free-solid-svg-icons';

interface Props {
  navClickCallback(nav: string): void;
}

// todo make each nav an enum or constant
const NavBarLeft = (props: Props) => {
  const [selectedNav, setSelectedNav] = useState<string | undefined>(undefined);

  const onNavClick = (nav: string) => {
    setSelectedNav(nav);
    props.navClickCallback(nav);
  };

  return (
    <div className="nav-left p-2">
      <Nav vertical>
        <NavItem active={selectedNav === 'Users'}>
          <NavLink href="#" onClick={() => onNavClick('Users')}>
            <FontAwesomeIcon icon={faUser} className="mr-2" fixedWidth={true} />
            Users
          </NavLink>
        </NavItem>
        <NavItem active={selectedNav === 'Suppliers'}>
          <NavLink href="#" onClick={() => onNavClick('Suppliers')}>
            <FontAwesomeIcon icon={faBuilding} className="mr-2" fixedWidth={true} />
            Suppliers
          </NavLink>
        </NavItem>
      </Nav>
    </div>
  );
};

export default NavBarLeft;
