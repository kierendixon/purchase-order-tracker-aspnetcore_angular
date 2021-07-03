import React, { useState } from 'react';
import './Site.scss';
import NavBarLeft from './narbar-left/NavBarLeft';
import NavBarTop from './navbar-top/NavBarTop';
import Users from './users-list/UsersList';

const Site = () => {
  // todo var name
  const [navMenu, setNavMenu] = useState<string>(window.location.pathname.substr('/admin/'.length));

  const onNavBarLeftClick = (nav: string) => {
    // todo: back button doesnt work with this approach
    window.history.pushState(null, nav, `/admin/${nav}`);
    setNavMenu(nav);
  };

  return (
    <>
      <NavBarTop />
      <div className="d-flex align-items-stretch site">
        <NavBarLeft clickHandler={(state) => onNavBarLeftClick(state)}></NavBarLeft>
        <MainContent navMenu={navMenu} />
      </div>
    </>
  );
};

// todo use path to select the component
// window.location.pathname => /search
// and move to its own component..
const MainContent = (props: { navMenu: string }) => {
  return (
    <div className="main-content flex-fill">
      {(() => {
        switch (props.navMenu.toLowerCase()) {
          case 'users':
            return <Users />;
          case 'suppliers':
            return <div>Suppliers List</div>;
          default:
            return null;
        }
      })()}
    </div>
  );
};

export default Site;
