import React, { useEffect, useState } from 'react';
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

const Users = (props) => {
  /*
  [header]
  create 
  edit
  list
  */
 const [mode, setMode]   = useState<string>("List");
 const [selectedRecord, setSelectedRecord] = useState<string | undefined>(undefined);

 const panel = ()=>{
    switch(mode) {
      case 'Create':
        return <span>Create</span>
      case 'Edit':
        return <span>Edit</span>
      case 'List':
        return <UsersList selectedRecord={selectedRecord} setSelectedRecord={setSelectedRecord}/>
    }
  }

  return (
    <>
      <UserButtons selectedRecord={selectedRecord} setSelectedRecord={setSelectedRecord} setMode={setMode} />
      {panel()}
    </>
  )
}

const UserButtons = (props) => {
  const {selectedRecord, setSelectedRecord, setMode}=props;

  return (
  <div
  className="users-list d-flex flex-column"
  onClick={() => {
    setSelectedRecord(undefined);
  }}
>
  <div className="menu d-flex">
    <Button className="btn" color="none" onClick={() => setMode('Create')}>
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
  </div>)
}

const UsersList = (props) => {
  const {selectedRecord, setSelectedRecord}=props;
  const [users, setUsers] = useState<Array<User> | undefined>(undefined);
  const [userFilter, setUserFilter] = useState<string | undefined>(undefined);

  useEffect(() => {
    async function fetchUsers() {
      var authService = new AuthService(new LocalStorageService(localStorage));
  
      let fetchData = {
        method: 'GET',
        headers: new Headers({ Authorization: `Bearer ${authService.currentUser?.token.access_token}` }),
      };
  
      var users = (await (await fetch('/admin/Users', fetchData)).json()) as UserResponse;
      setUsers(users.pagedList);
    }

    fetchUsers();
  }, []);

  function handleRowClick(id: string, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    setSelectedRecord(id);
    e.stopPropagation();
  }
  
  function filterMatchesUser(user: User): boolean {
    return userFilter == undefined
      ? false 
      : user.userName.toLowerCase().indexOf(userFilter.toLowerCase()) >= 0;
  }

  return (
    // fixed width ??
    // todo: make new ButtonWithIcon component
    <div>
      {users && (
          <div className="user-table ml-2 mt-2 flex-grow-1">
            <Form inline className="mb-2" onSubmit={(event) => event.preventDefault()}>
              <InputGroup>
                <Input placeholder="Filter" value={userFilter} onChange={(e) => setUserFilter(e.target.value)}/>
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
                  (userFilter === undefined || filterMatchesUser(user)) && 
                  <tr
                    onClick={(e) => {
                      handleRowClick(user.id, e);
                    }}
                    className={selectedRecord == user.id ? 'table-active' : ''}
                    key={user.id}
                  >
                    <th scope="row">{user.userName}</th>
                    <td>{user.twoFactorEnabled}</td>
                    <td>{user.refreshToken}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
      )}
    </div>
  );
};

export default Users;