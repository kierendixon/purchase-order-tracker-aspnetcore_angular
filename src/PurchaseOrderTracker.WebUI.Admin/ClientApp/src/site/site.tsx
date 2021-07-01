import React from 'react';
import './Site.scss';
import NavBarLeft from './narbar-left/NavBarLeft';
import NavBarTop from './navbar-top/NavBarTop';
import Users from './users-list/UsersList';

interface State {
  content: string | null;
}

export default class Site extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      content: null,
    };
  }

  render() {
    return (
      <>
        <NavBarTop />
        <div className="d-flex align-items-stretch site">
          <NavBarLeft clickHandler={this.onNavBarClick}></NavBarLeft>
          {this.renderMainContent()}
        </div>
      </>
    );
  }

  onNavBarClick = (nav: string) => {
    this.setState({ content: nav });
  };

  // todo use path to select the component
  // window.location.pathname => /search
  renderMainContent = () => (
    <div className="main-content flex-fill">
      {(() => {
        switch (this.state.content) {
          case 'Users':
            return <Users />;
          case 'Suppliers':
            return <div>Suppliers List</div>;
          default:
            return <div>default</div>; //null;
        }
      })()}
    </div>
  );
}
