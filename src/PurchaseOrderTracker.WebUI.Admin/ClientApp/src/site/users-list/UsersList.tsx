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
import { faEdit, faPlus, faTrash, faSearch, faUndo } from '@fortawesome/free-solid-svg-icons';

type UserResponse = {
  pagedList: Array<User>;
};

// TODO generate definitions from backend
interface User {
  id: string;
  userName: string;
  isAdmin: boolean;
  lockoutEnd: Date;
}

const Users = (props) => {
  /*
  [header]
  create 
  edit
  list
  */
  const [mode, setMode] = useState<string>('List');
  const [selectedUser, setSelectedUser] = useState<User | undefined>(undefined);

  // todo use enum / update name
  const panel = () => {
    switch (mode) {
      case 'Create':
        return <CreateUser submitCallback={() => setMode('List')} />;
      case 'Edit':
        return <EditUser onSubmit={() => setModeMode('List')} user={selectedUser!} />;
      case 'List':
        return <UsersList selectedUser={selectedUser} setSelectedUser={setSelectedUser} />;
    }
  };

  // todo
  const setModeMode = async (mode: string) => {
    // todo "delete" is not a mode but instead an action
    if (mode == 'Delete') {
      const response = await fetch(`/api/user/${selectedUser!.id}`, {
        method: 'DELETE',
      });

      if (response.status == 200) {
        // todo refresh user list. need to hoist up the user data
        // so that it can be refreshed and then passed to Userlist component
        setMode('Delete');
        setMode('List');
        setSelectedUser(undefined);
      } else {
        // handle error
      }
    } else {
      setMode(mode);

      if (mode == 'List') {
        setSelectedUser(undefined);
      }
    }
  };

  return (
    <>
      <ActionMenu selectedUser={selectedUser} setSelectedUser={setSelectedUser} mode={mode} setMode={setModeMode} />
      {panel()}
    </>
  );
};

const ActionMenu = (props) => {
  // todo define props
  const { selectedUser, setSelectedUser, mode, setMode } = props;

  return (
    <div
      className="users-list d-flex flex-column"
      // onClick={() => {
      //   setSelectedUser(undefined);
      // }}
    >
      <div className="menu d-flex">
        <Button className="btn" color="none" onClick={() => setMode('Create')}>
          <FontAwesomeIcon icon={faPlus} className="mr-1" fixedWidth={true} />
          New
        </Button>
        <div className="divider"></div>
        <Button disabled={selectedUser == null} color="none" className="btn" onClick={() => setMode('Edit')}>
          <FontAwesomeIcon icon={faEdit} className="mr-1" fixedWidth={true} />
          Edit
        </Button>
        <div className="divider"></div>
        <Button disabled={selectedUser == null} className="btn" color="none" onClick={() => setMode('Delete')}>
          <FontAwesomeIcon icon={faTrash} className="mr-1" fixedWidth={true} />
          Delete
        </Button>
        <div className="divider"></div>
        {(mode == 'Edit' || mode == 'Create') && (
          <Button className="btn" color="none" onClick={() => setMode('List')}>
            <FontAwesomeIcon icon={faUndo} className="mr-1" fixedWidth={true} />
            Cancel
          </Button>
        )}
        ;
      </div>
    </div>
  );
};

const CreateUser = (props: { submitCallback() }) => {
  // todo
  const [formValues, setFormValues] = useState<any>({});

  const onClickCreateUser = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const rawResponse = await fetch('/api/user', {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(formValues),
    });
    // const content = await rawResponse.json();

    if (rawResponse.status == 200) {
      props.submitCallback();
    } else {
      console.log(await rawResponse.json());
    }
  };

  const onFormInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    event.preventDefault();
    // any race condition concerns?
    setFormValues({ ...formValues, [event.target.name]: event.target.value });
  };

  const isFormValid = () => {
    return formValues.username == undefined && formValues.password == undefined;
  };

  return (
    <div style={{ padding: '20px', maxWidth: '500px' }}>
      <Form>
        <FormGroup row>
          <Label for="username" sm={4}>
            Username
          </Label>
          <Col sm={8}>
            <Input type="text" name="username" onChange={onFormInputChange} />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label for="onetimepassword" sm={4}>
            One time password
          </Label>
          <Col sm={8}>
            <Input type="password" name="onetimepassword" onChange={onFormInputChange} />
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
            {/* <Button color="primary" disabled={isFormValid()} onClick={(event) => onClickCreateUser(event)}>
              Create
            </Button> */}
          </Col>
        </FormGroup>
      </Form>
    </div>
  );
};

const EditUser = (props: { onSubmit(); user: User }) => {
  const [user, setUser] = useState<User>(Object.assign({}, props.user));

  const onSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const rawResponse = await fetch('/api/user', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(user),
    });
    // const content = await rawResponse.json();

    if (rawResponse.status == 200) {
      props.onSubmit();
    } else {
      console.log(await rawResponse.json());
    }
  };

  const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    // (e) => setUser(Object.assign({}, user, { [user.userName]: e.target.value }))
    const target = event.target;
    const value =
      target.type == 'checkbox'
        ? target.checked
        : target.type == 'datetime-local'
        ? Date.parse(target.value) || undefined
        : target.value;

    const property = event.target.name;

    setUser((prevUser) => ({
      ...prevUser,
      [property]: value,
    }));
  };

  const lockoutEndDateAsDateTimeLocal = () => {
    // 2021-07-08T15:24:07.0163509+10:00
    // todo hacky; use dayjs instead
    return props.user.lockoutEnd == null ? undefined : new Date(props.user.lockoutEnd).toISOString().substring(0, 23);
  };

  return (
    <div style={{ padding: '20px', maxWidth: '500px' }}>
      <Form>
        <FormGroup row>
          <Label for="id" sm={4}>
            Id
          </Label>
          <Col sm={8}>
            <Input type="text" name="id" value={user.id} disabled />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label for="userName" sm={4}>
            Username
          </Label>
          <Col sm={8}>
            <Input type="text" name="userName" value={user.userName} onChange={onChange} />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label for="lockoutEnd" sm={4}>
            Lockout End
          </Label>
          <Col sm={8}>
            <Input
              type="datetime-local"
              name="lockoutEnd"
              defaultValue={lockoutEndDateAsDateTimeLocal()}
              onChange={onChange}
            />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label sm={4} for="isAdmin"></Label>
          <Col sm={{ size: 8 }}>
            <FormGroup check>
              <Label check>
                <Input type="checkbox" name="isAdmin" checked={user.isAdmin} onChange={onChange} /> Admin
              </Label>
            </FormGroup>
          </Col>
        </FormGroup>
        <FormGroup check row>
          <Col sm={{ size: 8, offset: 4 }}>
            {/* <Button color="primary" disabled={false} onClick={(event) => onSubmit(event)}>
              Update
            </Button> */}
          </Col>
        </FormGroup>
      </Form>
    </div>
  );
};

const UsersList = (props) => {
  const { selectedUser, setSelectedUser } = props;
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

  function handleRowClick(user: User, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    setSelectedUser(user);
    e.stopPropagation(); // ??
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
              <Input placeholder="Search Username" value={userFilter} onChange={(e) => setUserFilter(e.target.value)} />
              <InputGroupAddon
                addonType="append"
                onClick={() => {
                  fetchUsers(userFilter);
                }}
                style={{ cursor: 'pointer' }}
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
                <th>Locked</th>
              </tr>
            </thead>
            <tbody>
              {users!.map(
                (user, i) =>
                  (userFilter === undefined || filterMatchesUser(user)) && (
                    <tr
                      onClick={(e) => {
                        handleRowClick(user, e);
                      }}
                      // todo use util function
                      className={(selectedUser?.id == user.id ? 'table-active' : '') + (i % 2 == 0 ? '' : '')}
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
                        <Input
                          // todo inline style
                          type="checkbox"
                          checked={user.lockoutEnd == undefined ? false : new Date(user.lockoutEnd) < new Date()}
                          readOnly
                          style={{ marginLeft: '0px' }}
                        />
                        {user.lockoutEnd == undefined
                          ? null
                          : new Date(user.lockoutEnd) < new Date()
                          ? ` (${user.lockoutEnd})`
                          : null}
                      </td>
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
