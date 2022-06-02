import React from 'react';
import userEvent from '@testing-library/user-event';
import { render, screen } from '@testing-library/react';

import NavBarLeft from '.';

describe('NavBarLeft', () => {
  test.each([['Users'], ['Suppliers']])('calls callback when %s nav selected', (nav: string) => {
    let callbackCounter = 0;
    let selectedNav;
    const callback = (nav: string) => {
      selectedNav = nav;
      callbackCounter++;
    };

    render(<NavBarLeft navClickCallback={callback} />);
    userEvent.click(screen.getByText(nav));

    expect(callbackCounter).toBe(1);
    expect(selectedNav).toBe(nav);
  });
});
