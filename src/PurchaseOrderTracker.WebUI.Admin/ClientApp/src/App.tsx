import React, { useState } from 'react';

import NavBarLeft from './features/NavBarLeft';
import NavBarTop from './features/NavBarTop';
import Suppliers from './features/Suppliers';
import Users from './features/Users';
import { adminWebsiteUrlPathPrefix } from './config/routing.config';

import './App.scss';
import './styles/globals.scss';

const getSelectedNavFromUrl = () => window.location.pathname.substr(adminWebsiteUrlPathPrefix.length);

const App = () => {
  const [selectedNav, setSelectedNav] = useState<string>(getSelectedNavFromUrl());

  const onNavItemClick = (nav: string) => {
    // todo: back button doesnt work with this approach
    window.history.pushState(null, nav, `${adminWebsiteUrlPathPrefix}${nav}`);
    setSelectedNav(nav);
  };

  return (
    <>
      <NavBarTop />
      <div className="d-flex align-items-stretch">
        <NavBarLeft navClickCallback={onNavItemClick}></NavBarLeft>
        <Content selectedNav={selectedNav} />
      </div>
    </>
  );
};

const Content = (props: { selectedNav: string }) => {
  return (
    <div className="content flex-fill">
      {(() => {
        switch (props.selectedNav.toLowerCase()) {
          case 'users':
            return <Users />;
          case 'suppliers':
            return <Suppliers />;
          default:
            return null;
        }
      })()}
    </div>
  );
};

export default App;
