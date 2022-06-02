import { Button, Col, Form, FormFeedback, FormGroup, Input, Label } from 'reactstrap';
import React, { useState } from 'react';

// todo move types to common class
import { User } from '../index';
import EditUserClient, { EditUserCommand } from './EditUserClient';
import Forms, { FormValue, FormValueMap } from '../../../lib/Forms';

interface Props {
  userEditedCallback();
  user: User;
}
interface FormValues extends FormValueMap {
  id: FormValue<string>;
  username: FormValue<string>;
  lockoutEnd: FormValue<string>;
  admin: FormValue<boolean>;
}

// todo fix date locale for lockoutEnd
// the value set is not the same as what is displayed
// todo write tests
const EditUser = (props: Props) => {
  const onFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!Forms.isFormValid(formValues)) {
      return;
    }

    const request = new EditUserCommand(
      formValues.id.value,
      formValues.username.value,
      formValues.admin.value,
      formValues.lockoutEnd.value === '' ? null : new Date(formValues.lockoutEnd.value)
    );

    const result = await EditUserClient.submit(request);

    if (result !== undefined) {
      if (result.success) {
        props.userEditedCallback();
      } else {
        // TODO handle errors
        // setAlertMessages(result.errors!);
        // setIsAlertVisible(true);
      }
    }
  };

  const [formValues, setFormValues] = useState<FormValues>({
    id: { value: props.user.id, errors: undefined, touched: false },
    username: { value: props.user.userName, errors: [], touched: false },
    lockoutEnd: { value: props.user.lockoutEnd?.toString() || '', errors: [], touched: false },
    admin: { value: props.user.isAdmin || false, touched: false },
  });

  const lockoutEndDateAsDateTimeLocal = (lockoutEnd: Date | null) => {
    // 2021-07-08T15:24:07.0163509+10:00
    // todo hacky; use dayjs instead
    return lockoutEnd == null ? undefined : new Date(lockoutEnd).toISOString().substring(0, 23);
  };

  // todo inline styles
  return (
    <div style={{ padding: '20px', maxWidth: '500px' }}>
      <Form onSubmit={onFormSubmit}>
        <FormGroup row>
          <Label for="id" sm={4}>
            Id
          </Label>
          <Col sm={8}>
            <Input type="text" name="id" defaultValue={props.user.id} disabled />
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label for="username" sm={4}>
            Username
          </Label>
          <Col sm={8}>
            <Input
              type="text"
              name="username"
              required
              minLength={3}
              defaultValue={props.user.userName}
              onFocus={(e) => Forms.onFormFieldFocus(e, formValues, setFormValues)}
              onChange={(e) => Forms.onFormInputChange(e, formValues, setFormValues)}
              invalid={formValues.username.touched && formValues.username.errors!.length !== 0}
            />
            {formValues.username.errors?.map((error) => (
              <FormFeedback key={error}>{error}</FormFeedback>
            ))}
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
              defaultValue={lockoutEndDateAsDateTimeLocal(props.user.lockoutEnd)}
              onFocus={(e) => Forms.onFormFieldFocus(e, formValues, setFormValues)}
              onChange={(e) => Forms.onFormInputChange(e, formValues, setFormValues)}
              invalid={formValues.username.touched && formValues.username.errors!.length !== 0}
            />
            {formValues.lockoutEnd.errors?.map((error) => (
              <FormFeedback key={error}>{error}</FormFeedback>
            ))}
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label sm={4} for="admin"></Label>
          <Col sm={{ size: 8 }}>
            <FormGroup check>
              <Label check>
                <Input
                  type="checkbox"
                  name="admin"
                  defaultChecked={props.user.isAdmin || false}
                  onChange={(e) => Forms.onFormInputChange(e, formValues, setFormValues)}
                />{' '}
                Admin
              </Label>
            </FormGroup>
          </Col>
        </FormGroup>
        <FormGroup check row>
          <Col sm={{ size: 8, offset: 4 }}>
            <Button color="primary" disabled={!Forms.isFormValid(formValues)} type="submit">
              Update
            </Button>
          </Col>
        </FormGroup>
      </Form>
    </div>
  );
};

export default EditUser;
