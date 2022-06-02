import React from 'react';
import { render, screen } from '@testing-library/react';

import NavBarTop from '.';

describe('NavBarTop', () => {
  test('renders', () => {
    render(<NavBarTop />);
    const navbarTitle = screen.getByText(/Purchase Order Tracker - Administration/i);
    expect(navbarTitle).toBeInTheDocument();
  });
});
