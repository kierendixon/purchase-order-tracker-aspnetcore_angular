import React from 'react';
import './site.scss';
import NavBarLeft from './narbar-left/navbar-left';
import NavBarTop from './navbar-top/navbar-top';

import UserList from '../users/users-list/users-list';
type ClockState = {
  time: Date
}
export default class Site extends React.Component<{}, State> {
  constructor(props) {
    super(props);
    this.state = {
      content: null
    };
  }

  render() {
    return (
      <>
      <NavBarTop/>
      <div className="d-flex align-items-stretch site">
        <NavBarLeft clickHandler={this.navClickHandler}></NavBarLeft>
        {this.renderMainContent()}
      </div>
      </>
    );
  }

  navClickHandler = (nav: string) =>
  {
    this.setState({content: nav});
  }

  renderMainContent = () =>
    (
      <div className="main-content flex-fill">
        {(() => {
          switch (this.state.content) {
            case 'Users':
              return <UserList/>;
            case 'Suppliers':
              return <div>Suppliers List</div>;
            default:
              return <div>default</div>//null;
          }
        })()}
      </div>
    );

}

interface State {
  content: string | null;
}
