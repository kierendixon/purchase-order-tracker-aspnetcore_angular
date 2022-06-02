import React from 'react';
import ReactDOM from 'react-dom';
// todo does this mean package.json is bundled into prod build?
//
//
//
//
///
import { homepage } from '../package.json';
import reportWebVitals from './reportWebVitals';

import App from './App';

import './styles/bootstrap2.scss';

async function main() {
  if (process.env.REACT_APP_USE_MSW === '1') {
    if (window.location.pathname === homepage) {
      window.location.pathname = `${homepage}/`;
      return;
    }
    const { worker } = require('./stubs/browser');
    await worker.start({
      serviceWorker: {
        url: `${homepage}/mockServiceWorker.js`,
      },
    });
  }

  ReactDOM.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
    document.getElementById('root')
  );
}

main();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
