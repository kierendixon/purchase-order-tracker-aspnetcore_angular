import React from 'react';
import { render, screen } from '@testing-library/react';

import App from './App';

test('renders', () => {
  render(<App />);
  const navbarTitle = screen.getByText(/Purchase Order Tracker - Administration/i);
  expect(navbarTitle).toBeInTheDocument();
});
