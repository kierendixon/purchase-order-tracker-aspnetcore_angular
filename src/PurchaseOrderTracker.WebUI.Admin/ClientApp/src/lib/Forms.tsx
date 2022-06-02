import React from 'react';

// todo look at formik or react-hook-form for form management
interface FormValue<T> {
  value: T;
  touched: boolean;
  errors?: string[];
}

interface FormValueMap {
  [name: string]: FormValue<string | boolean | string[]>;
}

function onFormFieldFocus<T extends FormValueMap>(
  event: React.FocusEvent<HTMLInputElement>,
  formValues: T,
  setFormValues: React.Dispatch<React.SetStateAction<T>>
) {
  // only set touched if not already set
  if (formValues[event.target.name].touched !== true) {
    setFormValues({ ...formValues, [event.target.name]: { ...formValues[event.target.name], touched: true } });
  }
}

function validateInput(eventTarget: EventTarget & HTMLInputElement): string[] {
  const value = getValueFromTarget(eventTarget);
  const errors: string[] = [];

  if (typeof value === 'string') {
    if (eventTarget.required && (value === '' || value.trim() === '')) {
      errors.push('This field is required');
    }

    if (value !== null && value.trim() !== '' && value.length < eventTarget.minLength) {
      errors.push(`Minimum length ${eventTarget.minLength}`);
    }
  }

  return errors;
}

function onFormInputChange<T extends FormValueMap>(
  event: React.ChangeEvent<HTMLInputElement>,
  formValues: T,
  setFormValues: React.Dispatch<React.SetStateAction<T>>
) {
  const value = getValueFromTarget(event.target);
  const errors = validateInput(event.target);
  setFormValues({ ...formValues, [event.target.name]: { value, errors, touched: true } });
}

function getValueFromTarget(eventTarget: EventTarget & HTMLInputElement) {
  return eventTarget.type === 'checkbox' ? eventTarget.checked : eventTarget.value;
}

function isFormValid(formValues: FormValueMap) {
  return !Object.entries(formValues)
    .filter((fv) => fv[1].errors !== undefined)
    .some((fv) => fv[1].errors!.length > 0);
}

const forms = { onFormFieldFocus, validateInput, onFormInputChange, isFormValid };

export type { FormValue, FormValueMap };
export default forms;
