import { Alert, Button, Col, Form, FormFeedback, FormGroup, Input, Label } from 'reactstrap';
import React, { useState } from 'react';

import CreateUserClient, { CreateCommand } from './CreateUserClient';
import Forms, { FormValue, FormValueMap } from '../../../lib/Forms';

interface FormValues extends FormValueMap {
  username: FormValue<string>;
  onetimepassword: FormValue<string>;
  admin: FormValue<boolean>;
}

// todo new or create for name?
// todo write tests
const NewUser = (props: { userCreatedCallback(userId: number) }) => {
  const [isAlertVisible, setIsAlertVisible] = useState(false);
  const [alertMessages, setAlertMessages] = useState<string[]>([]);

  const onFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setIsAlertVisible(false);

    if (Forms.isFormValid(formValues)) {
      const command = new CreateCommand(
        formValues.username.value,
        formValues.onetimepassword.value,
        formValues.admin.value
      );

      const result = await CreateUserClient.submit(command);

      if (result !== undefined) {
        if (result.success) {
          props.userCreatedCallback(result.userId!);
        } else {
          setAlertMessages(result.errors!);
          setIsAlertVisible(true);
        }
      }
    }
  };

  const [formValues, setFormValues] = useState<FormValues>({
    username: { value: '', errors: [], touched: false },
    onetimepassword: { value: '', errors: [], touched: false },
    admin: { value: false, touched: false },
  });

  // todo highlight invalid input
  // todo inline style
  // todo it should be possible to submit the form without touching the isAdmin field
  // todo move alert higher up the chain to make it easier to reuse
  return (
    <>
      <Alert color="danger" isOpen={isAlertVisible} toggle={() => setIsAlertVisible(false)}>
        <ul>
          {alertMessages.map((msg) => (
            <li>{msg}</li>
          ))}
        </ul>
      </Alert>
      <Form onSubmit={onFormSubmit} style={{ padding: '20px', maxWidth: '500px' }}>
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
          <Label for="onetimepassword" sm={4}>
            One time password
          </Label>
          <Col sm={8}>
            <Input
              type="password"
              name="onetimepassword"
              required
              minLength={3}
              onChange={(e) => Forms.onFormInputChange(e, formValues, setFormValues)}
              onFocus={(e) => Forms.onFormFieldFocus(e, formValues, setFormValues)}
              invalid={formValues.onetimepassword.touched && formValues.onetimepassword.errors!.length !== 0}
            />
            {formValues.onetimepassword.errors?.map((error) => (
              <FormFeedback key={error}>{error}</FormFeedback>
            ))}
          </Col>
        </FormGroup>
        <FormGroup row>
          <Label sm={4}></Label>
          <Col sm={{ size: 8 }}>
            <FormGroup check>
              <Label check>
                <Input
                  type="checkbox"
                  name="admin"
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
              Create
            </Button>
          </Col>
        </FormGroup>
      </Form>
    </>
  );
};

export default NewUser;
