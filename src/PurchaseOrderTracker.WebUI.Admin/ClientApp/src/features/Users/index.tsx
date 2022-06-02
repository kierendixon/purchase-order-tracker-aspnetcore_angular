// todo get socketjs working behind proxy
import React, { useEffect, useState } from 'react';

import EditUser from './EditUser/EditUser';
import NewUser from './CreateUser/NewUser';
import UserList from './UserList/UserList';
import ActionMenu, { Action } from './ActionMenu';

import './UserList/UsersList.scss';

// todo move
type UserResponse = {
  pagedList: Array<User>;
};

// TODO generate definitions from backend
interface User {
  id: string;
  userName: string;
  isAdmin: boolean | null;
  lockoutEnd: Date | null;
}

// todo write tests
const Users = () => {
  const [selectedAction, setSelectedAction] = useState<Action | undefined>(undefined);
  const [selectedUser, setSelectedUser] = useState<User | undefined>(undefined);
  const [users, setUsers] = useState<User[] | undefined>(undefined);

  // todo move out of component
  const deleteUser = async (id: string) => {
    // todo: on 401 redirect to logon page or display error message (e.g. "session timed out" -> + clear cookie)
    const response = await fetch(`/api/user/${selectedUser!.id}`, {
      method: 'DELETE',
    });

    return response.status === 200;
  };

  const onActionMenuItemClick = async (action: Action) => {
    if (action === Action.Delete) {
      const isSuccessful = await deleteUser(selectedUser!.id);

      if (isSuccessful) {
        setSelectedUser(undefined);
        await fetchUsers();
      } else {
        // todo handle error
      }
    } else {
      setSelectedAction(action);

      // todo don't clear selected user when cancelling an edit or new
      if (action === Action.Create || action === Action.Cancel) {
        setSelectedUser(undefined);
      }
    }
  };

  const refetchUsers = async () => {
    setSelectedAction(undefined);
    setSelectedUser(undefined);
    fetchUsers();
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  // todo move to another file
  const fetchUsers = async (filter?: string) => {
    let url = '/api/user?pageSize=20';
    if (filter !== undefined && filter.trim().length > 0) {
      url += `&filter=${encodeURIComponent(filter)}`;
    }

    // todo handle error response
    const users = (await (await fetch(url)).json()) as UserResponse;
    setUsers(users.pagedList);
  };

  // todo update loading text to a spinner
  return (
    <>
      <ActionMenu
        isUserSelected={selectedUser !== undefined}
        isEditing={selectedAction === Action.Edit || selectedAction === Action.Create}
        menuItemClickCallback={onActionMenuItemClick}
      />
      {(() => {
        switch (selectedAction) {
          case Action.Create:
            return <NewUser userCreatedCallback={refetchUsers} />;
          case Action.Edit:
            return <EditUser userEditedCallback={refetchUsers} user={selectedUser!} />;
          default:
            return users == undefined ? (
              <span>Loading...</span>
            ) : (
              <UserList users={users} filterUsersCallback={fetchUsers} selectedUserCallback={setSelectedUser} />
            );
        }
      })()}
    </>
  );
};

export type { User };
export default Users;
