import React from 'react';
import './navbar-left.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faHome, faBuilding } from '@fortawesome/free-solid-svg-icons';
import { Nav, NavItem, NavLink, Collapse, Button } from 'reactstrap';



// rename to sidebar?
export default class NavBarLeft extends React.Component<Props, State> {
  constructor(props) {
    super(props);
    this.toggle = this.toggle.bind(this);
    this.handleNavSelect = this.handleNavSelect.bind(this);
    this.state = { collapse: true, activeNavItem: null };
  }

  //TODO
  toggle() {
    this.setState(state => ({ collapse: !state.collapse }));
  }

  handleNavSelect(nav) {
    console.log('nav: ' + nav);
    this.setState(state => ({ ...this.state, activeNavItem: nav }));
    this.props.clickHandler(nav);
  }

  render() {
    return (
      <div className="nav-left p-2 h-100">
        <Collapse isOpen={this.state.collapse}>
          <Nav vertical>
            <NavItem active>
              <NavLink href="#">
                <FontAwesomeIcon icon={faHome} className="mr-2" fixedWidth={true} />
                Home
              </NavLink>
            </NavItem>
          <hr className="navbar-divider my-0" />
            <NavItem active={this.state.activeNavItem === 'Users'}>
              <NavLink href="#" onClick={() => this.handleNavSelect('Users')}>
                <FontAwesomeIcon icon={faUser} className="mr-2" fixedWidth={true} />
                Users
              </NavLink>
            </NavItem>
            <NavItem active={this.state.activeNavItem === 'Suppliers'}>
              <NavLink href="#" onClick={() => this.handleNavSelect('Suppliers')}>
                <FontAwesomeIcon icon={faBuilding} className="mr-2" fixedWidth={true} />
                Suppliers
              </NavLink>
            </NavItem>
          </Nav>
          <hr className="navbar-divider my-0" />
          <Button color="primary" onClick={this.toggle} style={{ marginBottom: '1rem', display: 'none' }}>
            Toggle
          </Button>
        </Collapse>
      </div>
    );
  }
}

interface State {
  collapse: boolean;
  activeNavItem: string | null;
}

interface Props {
  clickHandler: Function;
}
