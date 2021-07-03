import React from 'react';
import './NavBarTop.scss';
import logo from './logo.png';
// import { Button } from 'reactstrap';
import Logout from '../logout/Logout';

export default class NavBarTop extends React.Component {
  render() {
    return (
      <div className="navbar-top d-flex justify-content-between">
        <div className="header d-flex justify-content-center">
          <img src={logo} alt="logo" />
          <span className="mx-3 pt-1">PoT ADMIN</span>
        </div>
        <div>
          {/* todo */}
          <span
            style={{ cursor: 'pointer' }}
            className="header pr-4"
            onClick={() => (window.location.href = '/main-site')}
          >
            Back to Main Site
          </span>
        </div>
        <Logout className="header pr-4"></Logout>
      </div>
    );
  }
}
