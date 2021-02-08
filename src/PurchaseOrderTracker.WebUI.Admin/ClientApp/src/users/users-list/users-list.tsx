import React, { Component, useEffect } from 'react';
import { Table, Button, InputGroupAddon, InputGroup, Input, Form, InputGroupText } from 'reactstrap';
import './users-list.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faPlus, faTrash, faSearch } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../site/security/AuthService';
import { LocalStorageService } from '../../site/security/LocalStorageService';

type UserResponse = {
  pagedList: Array<User>;
};

type User = {
  id: string;
  userName: string;
  twoFactorEnabled: boolean;
  refreshToken: string;
};

const UsersList = (props) => {
  const [selectedRecord, setSelectedRecord] = React.useState<string | null>(null);
  const [users, setUsers] = React.useState<Array<User> | null>(null);
  const [userFilter, setUserFilter] = React.useState<string | null>(null);

  function handleRowClick(id: string, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    setSelectedRecord(id);
    e.stopPropagation();
  }

  async function getUsers() {
    var authService = new AuthService(new LocalStorageService(localStorage));

    let fetchData = {
      method: 'GET',
      headers: new Headers({ Authorization: `Bearer ${authService.currentUser?.token.access_token}` }),
    };

    var users = (await (await fetch('/admin/User', fetchData)).json()) as UserResponse;
    setUsers(users.pagedList);
  }

  useEffect(() => {
    getUsers();
  }, []);

  return (
    // fixed width ??
    // todo: make new ButtonWithIcon component
    <div>
      {users && (
        <div
          className="users-list d-flex flex-column"
          onClick={() => {
            setSelectedRecord(null);
          }}
        >
          <div className="menu d-flex">
            <Button className="btn" color="none">
              <FontAwesomeIcon icon={faPlus} className="mr-1" fixedWidth={true} />
              New
            </Button>
            <div className="divider"></div>
            <Button disabled={selectedRecord == null} color="none" className="btn">
              <FontAwesomeIcon icon={faEdit} className="mr-1" fixedWidth={true} />
              Edit
            </Button>
            <div className="divider"></div>
            <Button disabled={selectedRecord == null} className="btn" color="none">
              <FontAwesomeIcon icon={faTrash} className="mr-1" fixedWidth={true} />
              Delete
            </Button>
            <div className="divider"></div>
          </div>
userftiler:{userFilter}
          <div className="user-table ml-2 mt-2 flex-grow-1">
            <Form inline className="mb-2">
              <InputGroup>
                <Input placeholder="Filter" onChange={(e) => setUserFilter(e.target.value)}/>
                <InputGroupAddon addonType="append">
                  <InputGroupText>
                    <FontAwesomeIcon icon={faSearch} className="mr-1" fixedWidth={true} />
                  </InputGroupText>
                </InputGroupAddon>
              </InputGroup>
            </Form>
            <Table hover>
              <thead>
                <tr>
                  <th>Username</th>
                  <th>Two Factor Enabled</th>
                  <th>Refresh Token</th>
                </tr>
              </thead>
              <tbody>
                {users!.map((user) => (
                  <tr
                    onClick={(e) => {
                      handleRowClick(user.id, e);
                    }}
                    className={selectedRecord == user.id ? 'table-active' : ''}
                  >
                    <th scope="row">{user.userName}</th>
                    <td>{user.twoFactorEnabled}</td>
                    <td>{user.refreshToken}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
        </div>
      )}
    </div>
  );
};

export default UsersList;
