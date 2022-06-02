import React from 'react';
import userEvent from '@testing-library/user-event';
import { render, screen } from '@testing-library/react';

import ActionMenu, { Action } from './ActionMenu';

describe('Action<Menu', () => {
  test('shows cancel button when editing', () => {
    const menu = new ActionMenuBuilder().isEditing(true).build();
    render(menu);

    const cancelButton = screen.getByText('Cancel');
    expect(cancelButton).not.toBeNull();
  });

  test.each([['New'], ['Edit'], ['Delete']])('Disables %s button when editing', (buttonName: string) => {
    const menu = new ActionMenuBuilder().isEditing(true).build();
    render(menu);

    const button = screen.getByText(buttonName);
    expect(button.className).toContain('disabled');
  });

  test.each([['Edit'], ['Delete']])('Enables %s button when user is selected', (buttonName: string) => {
    const menu = new ActionMenuBuilder().isUserSelected(true).build();
    render(menu);

    const button = screen.getByText(buttonName);
    expect(button.className).not.toContain('disabled');
  });

  test.each([['Edit'], ['Delete']])('Disables %s button when no user is selected', (buttonName: string) => {
    const menu = new ActionMenuBuilder().isUserSelected(false).build();
    render(menu);

    const button = screen.getByText(buttonName);
    expect(button.className).toContain('disabled');
  });

  test.each([
    ['New', 'create', false, false],
    ['Edit', 'edit', true, false],
    ['Delete', 'delete', true, false],
    ['Cancel', 'cancel', false, true],
  ])(
    'Calls callback when %s button is clicked',
    async (buttonName: string, callbackButtonName: string, isUserSelected: boolean, isEditing: boolean) => {
      let callbackValue: Action;
      const menu = new ActionMenuBuilder()
        .isUserSelected(isUserSelected)
        .isEditing(isEditing)
        .menuItemCallback((action) => (callbackValue = action))
        .build();
      render(menu);

      await userEvent.click(screen.getByText(buttonName));

      expect(callbackValue).toBe(callbackButtonName);
    }
  );
});

class ActionMenuBuilder {
  constructor(
    private _isEditing = false,
    private _isUserSelected = false,
    private _menuItemCallback: (action: Action) => void = () => {}
  ) {}

  isEditing(isEditing: boolean) {
    this._isEditing = isEditing;
    return this;
  }

  isUserSelected(isUserSelected: boolean) {
    this._isUserSelected = isUserSelected;
    return this;
  }

  menuItemCallback(menuItemCallback: (action: Action) => void) {
    this._menuItemCallback = menuItemCallback;
    return this;
  }

  build() {
    return (
      <ActionMenu
        isEditing={this._isEditing}
        menuItemClickCallback={this._menuItemCallback}
        isUserSelected={this._isUserSelected}
      />
    );
  }
}
