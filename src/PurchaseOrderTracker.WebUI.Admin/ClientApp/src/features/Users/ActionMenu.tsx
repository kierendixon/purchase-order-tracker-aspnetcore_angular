import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React from 'react';
import { Nav, NavItem, NavLink } from 'reactstrap';
import { faEdit, faPlus, faTrash, faUndo } from '@fortawesome/free-solid-svg-icons';

// todo should have its own css
import './UserList/UsersList.scss';

interface Props {
  isUserSelected: boolean;
  isEditing: boolean;
  menuItemClickCallback(action: Action);
}

enum Action {
  Create = 'create',
  Edit = 'edit',
  Delete = 'delete',
  Cancel = 'cancel',
}

const ActionMenu = ({ menuItemClickCallback, isUserSelected, isEditing }: Props) => {
  const onActionClick = (action: Action) => {
    menuItemClickCallback(action);
  };

  return (
    <Nav className="users-list bg-hero">
      <NavItem>
        <NavLink href="#" disabled={isEditing} onClick={() => onActionClick(Action.Create)}>
          <FontAwesomeIcon icon={faPlus} className="mr-1" fixedWidth={true} />
          New
        </NavLink>
      </NavItem>
      <div className="divider" />
      <NavItem>
        <NavLink disabled={!isUserSelected || isEditing} href="#" onClick={() => onActionClick(Action.Edit)}>
          <FontAwesomeIcon icon={faEdit} className="mr-1" fixedWidth={true} />
          Edit
        </NavLink>
      </NavItem>
      <div className="divider" />
      <NavItem>
        <NavLink disabled={!isUserSelected || isEditing} href="#" onClick={() => onActionClick(Action.Delete)}>
          <FontAwesomeIcon icon={faTrash} className="mr-1" fixedWidth={true} />
          Delete
        </NavLink>
      </NavItem>
      <div className="divider" />
      {isEditing && (
        <NavItem>
          <NavLink href="#" onClick={() => onActionClick(Action.Cancel)}>
            <FontAwesomeIcon icon={faUndo} className="mr-1" fixedWidth={true} />
            Cancel
          </NavLink>
        </NavItem>
      )}
    </Nav>
  );
};

export { Action };
export default ActionMenu;
