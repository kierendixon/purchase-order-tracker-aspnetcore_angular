import React from 'react';
import ReactDOM from 'react-dom';
import UsersList from './users-list';

it('renders', () => {
  const div = document.createElement('div');
  ReactDOM.render(<UsersList />, div);
  ReactDOM.unmountComponentAtNode(div);
});
