import React from 'react';
import { render, screen } from '@testing-library/react';

import Suppliers from '.';

describe('Suppliers', () => {
  test('renders', () => {
    render(<Suppliers />);
    const navbarTitle = screen.getByText(/TODO:/i);
    expect(navbarTitle).toBeInTheDocument();
  });
});
