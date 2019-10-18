import React, { Component } from 'react';
import { Table, Button, InputGroupAddon, InputGroup, Input, Form, InputGroupText } from 'reactstrap';
import './users-list.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faPlus, faTrash, faSearch } from '@fortawesome/free-solid-svg-icons';

// class Button extends Component<{}, State> {
//   constructor(props) {
//     super(props);
//     this.state = {
//       name: "john"
//     };
//   }

//   render = () => (
//     <div>button {this.state.name}</div>
//   )
// }

// export default Button;

// type State {
//   name: string;
// }

const UsersList = props => {
  const [selectedRecord, setSelectedRecord] = React.useState<number | null>(null);

  function handleRowClick(id: number, e: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    setSelectedRecord(id);
    e.stopPropagation();
  }

  return (
    // fixed width ??
    // todo: make new ButtonWithIcon component
    <div className="users-list d-flex flex-column" onClick={() => setSelectedRecord(null)}>
      <div className="menu d-flex">
        <Button className="btn" color="none">
          <FontAwesomeIcon icon={faPlus} className="mr-1" fixedWidth={true} />
          New
        </Button>
        <div className="divider"></div>
        <Button disabled={selectedRecord==null} color="none" className="btn">
          <FontAwesomeIcon icon={faEdit} className="mr-1" fixedWidth={true} />
          Edit
        </Button>
        <div className="divider"></div>
        <Button disabled={selectedRecord==null}  className="btn" color="none">
          <FontAwesomeIcon icon={faTrash} className="mr-1" fixedWidth={true} />
          Delete
        </Button>
        <div className="divider"></div>
      </div>

      <div className="user-table ml-2 mt-2 flex-grow-1">
        <Form inline className="mb-2">
          <InputGroup>
            <Input placeholder="Filter" />
            <InputGroupAddon addonType="append">
          <InputGroupText><FontAwesomeIcon icon={faSearch} className="mr-1" fixedWidth={true} /></InputGroupText>
        </InputGroupAddon>
          </InputGroup>
        </Form>
        <Table hover>
          <thead>
            <tr>
              <th>#</th>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Username</th>
            </tr>
          </thead>
          <tbody>
          <tr onClick={(e) =>{ handleRowClick(1, e); }} className={ selectedRecord == 1 ? 'table-active' : ''}>
              <th scope="row">1</th>
              <td>Mark</td>
              <td>Otto</td>
              <td>@mdo</td>
            </tr>
            <tr onClick={(e) =>{ handleRowClick(2, e); }} className={ selectedRecord == 2 ? 'table-active' : ''}>
              <th scope="row">2</th>
              <td>Jacob</td>
              <td>Thornton</td>
              <td>@fat</td>
            </tr>
            <tr onClick={(e) =>{ handleRowClick(3, e); }} className={ selectedRecord == 3 ? 'table-active' : ''}>
              <th scope="row">3</th>
              <td>Larry</td>
              <td>the Bird</td>
              <td>@twitter</td>
            </tr>
          </tbody>
        </Table>
      </div>
    </div>
  );
};

export default UsersList;
