// todo get socketjs working behind proxy
import React, { useEffect, useState } from 'react';
import {
  Table,
  Button,
  InputGroupAddon,
  InputGroup,
  Input,
  Form,
  InputGroupText,
  FormGroup,
  Label,
  Col,
} from 'reactstrap';
import './UsersList.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faPlus, faTrash, faSearch } from '@fortawesome/free-solid-svg-icons';

type UserResponse = {
  pagedList: Array<User>;
};

// TODO generate definitions from backend
type User = {
  id: string;
  userName: string;
  isAdmin: boolean;
  lockoutEnabled: boolean;
  lockoutEnd: Date;
};

const Users = (props) => {
  /*
  [header]
  create 
  edit
  list
  */
  const [mode, setMode] = useState<string>('List');
  const [selectedRecord, setSelectedRecord] = useState<string | undefined>(undefined);

  const panel = () => {
    switch (mode) {
      case 'Create':
        return <CreateUser submitCallback={() => setMode('List')} />;
      case 'Edit':
        return <span>Edit</span>;
      case 'List':
        return <UsersList selectedRecord={selectedRecord} setSelectedRecord={setSelectedRecord} />;
    }
  };

  return (
    <>
      <UserButtons selectedRecord={selectedRecord} setSelectedRecord={setSelectedRecord} setMode={setMode} />
      {panel()}
    </>
  );
};

const UserButtons = (props) => {
  const { selectedRecord, setSelectedRecord, setMode } = props;

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
        <Button disabled={selectedRecord == null} color="none" className="btn" onClick={() => setMode('Edit')}>
          <FontAwesomeIcon icon={faEdit} className="mr-1" fixedWidth={true} />
          Edit
        </Button>
        <div className="divider"></div>
        <Button disabled={selectedRecord == null} className="btn" color="none" onClick={() => setMode('Delete')}>
          <FontAwesomeIcon icon={faTrash} className="mr-1" fixedWidth={true} />
          Delete
        </Button>
        <div className="divider"></div>
      </div>
    </div>
  );
};

const CreateUser = (props) => {
  const [formValues, setFormValues] = useState<any>({});

  function handleOnClickCreateUser(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    console.log(formValues);
    props.submitCallback();
  }

  const handleonChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    event.preventDefault();
    // any race condition concerns?
    setFormValues({ ...formValues, [event.target.name]: event.target.value });
  };

  return (
    <div style={{ padding: '20px', maxWidth: '500px' }}>
      <Form>
        <FormGroup row>
          <Label for="username" sm={4}>
            Username
          </Label>
          <Col sm={8}>
            <Input type="text" name="username" onChange={handleonChange} />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label for="password" sm={4}>
            One time password
          </Label>
          <Col sm={8}>
            <Input type="password" name="password" onChange={handleonChange} />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label sm={4}></Label>
          <Col sm={{ size: 8 }}>
            <FormGroup check>
              <Label check>
                <Input type="checkbox" /> Admin
              </Label>
            </FormGroup>
          </Col>
        </FormGroup>
        <FormGroup check row>
          <Col sm={{ size: 8, offset: 4 }}>
            <Button color="primary" onClick={(event) => handleOnClickCreateUser(event)}>
              Create
            </Button>
          </Col>
        </FormGroup>
      </Form>
    </div>
  );
};

const UsersList = (props) => {
  const { selectedRecord, setSelectedRecord } = props;
  const [users, setUsers] = useState<Array<User> | undefined>(undefined);
  const [userFilter, setUserFilter] = useState<string | undefined>(undefined);

  async function fetchUsers(filter?: string) {
    // todo url encode
    const url = filter ? `/api/user?pageSize=20&filter=${filter}` : '/api/user?pageSize=20';
    var users = (await (await fetch(url)).json()) as UserResponse;
    setUsers(users.pagedList);
  }

  useEffect(() => {
    fetchUsers();
  }, []);

  function handleRowClick(id: string, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    setSelectedRecord(id);
    e.stopPropagation();
  }

  function filterMatchesUser(user: User): boolean {
    return true;
    // return userFilter == undefined
    //   ? false
    //   : user.userName.toLowerCase().indexOf(userFilter.toLowerCase()) >= 0;
  }

  function handleSearchFormSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    fetchUsers(userFilter);
  }

  return (
    // fixed width ??
    // todo: make new ButtonWithIcon component
    <div>
      {users && (
        <div className="user-table ml-2 mt-2 flex-grow-1">
          <Form inline className="mb-2" onSubmit={(event) => handleSearchFormSubmit(event)}>
            <InputGroup>
              <Input
                placeholder="Search Username"
                value={userFilter}
                onChange={(e) => setUserFilter(e.target.value)}
                style={{ cursor: 'pointer' }}
              />
              <InputGroupAddon
                addonType="append"
                onClick={() => {
                  fetchUsers(userFilter);
                }}
              >
                <InputGroupText>
                  <FontAwesomeIcon icon={faSearch} className="mr-1" fixedWidth={true} />
                </InputGroupText>
              </InputGroupAddon>
            </InputGroup>
          </Form>
          <Table hover striped>
            <thead>
              <tr>
                <th>Username</th>
                <th>Admin</th>
                <th>Locked Out</th>
                <th>Lockout End</th>
              </tr>
            </thead>
            <tbody>
              {users!.map(
                (user, i) =>
                  (userFilter === undefined || filterMatchesUser(user)) && (
                    <tr
                      onClick={(e) => {
                        handleRowClick(user.id, e);
                      }}
                      // todo use util function
                      className={(selectedRecord == user.id ? 'table-active' : '') + (i % 2 == 0 ? '' : '')}
                      key={user.id}
                    >
                      <th scope="row">{user.userName}</th>
                      <td>
                        <Input
                          // todo inline style
                          type="checkbox"
                          checked={user.isAdmin}
                          readOnly
                          style={{ marginLeft: '0px' }}
                        />
                      </td>
                      <td>
                        <Input type="checkbox" checked={user.lockoutEnabled} readOnly style={{ marginLeft: '0px' }} />
                      </td>
                      <td>{user.lockoutEnd}</td>
                    </tr>
                  )
              )}
            </tbody>
          </Table>
        </div>
      )}
    </div>
  );
};

export default Users;
