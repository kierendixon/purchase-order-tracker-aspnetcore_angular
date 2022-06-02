import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Form, Input, InputGroup, InputGroupAddon, InputGroupText, Label, Table } from 'reactstrap';
import React, { useState } from 'react';

import { User } from '../index';

interface Props {
  users: User[];
  filterUsersCallback(filter?: string);
  selectedUserCallback(user?: User);
}

// todo write tests
const UserList = ({ users, filterUsersCallback, selectedUserCallback }: Props) => {
  const [selectedUser, setSelectedUser] = useState<User | undefined>(undefined);
  const [userFilter, setUserFilter] = useState<string>('');

  const handleRowClick = (user: User, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) => {
    setSelectedUser(user);
    selectedUserCallback(user);
  };

  const handleSearchFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    filterUsersCallback(userFilter);
  };

  const isUserLockedOut = (user: User) => user.lockoutEnd !== null && new Date(user.lockoutEnd) > new Date();

  return (
    <>
      {users && (
        <div className="user-table ml-2 mt-2 flex-grow-1">
          <Form inline className="mb-2" onSubmit={handleSearchFormSubmit}>
            <InputGroup>
              <Input placeholder="Search Username" value={userFilter} onChange={(e) => setUserFilter(e.target.value)} />
              <InputGroupAddon
                addonType="append"
                onClick={() => filterUsersCallback(userFilter)}
                className="cursor-pointer"
              >
                <InputGroupText>
                  <FontAwesomeIcon icon={faSearch} fixedWidth={true} />
                </InputGroupText>
              </InputGroupAddon>
            </InputGroup>
          </Form>
          <Table hover striped>
            <thead>
              <tr>
                <th>Username</th>
                <th>Admin</th>
                <th>Locked</th>
              </tr>
            </thead>
            <tbody>
              {users!.map((user) => (
                <tr
                  key={user.id}
                  onClick={(e) => {
                    handleRowClick(user, e);
                  }}
                  className={selectedUser?.id === user.id ? 'table-active' : ''}
                >
                  <th scope="row">{user.userName}</th>
                  <td>
                    <Input type="checkbox" checked={user.isAdmin === true} addon readOnly />
                  </td>
                  <td>
                    <Label check>
                      <Input type="checkbox" checked={isUserLockedOut(user)} addon readOnly />
                      {isUserLockedOut(user) && <span>{' ' + new Date(user.lockoutEnd!).toLocaleString()}</span>}
                    </Label>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        </div>
      )}
    </>
  );
};

export default UserList;
