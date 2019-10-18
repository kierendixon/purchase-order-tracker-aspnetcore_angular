import React from 'react';
import './navbar-top.scss';
import logo from './logo.png';
// import { Button } from 'reactstrap';

export default class NavBarTop extends React.Component {
  render() {
    return (
      <div className="navbar-top d-flex justify-content-between">
        <div className="header d-flex justify-content-center">
          <img src={logo} alt="logo" />
          <span className="mx-3 pt-1">PoT ADMIN</span>
        </div>
        <div className="header pr-4">Logout</div>
      </div>
    );
  }
}
