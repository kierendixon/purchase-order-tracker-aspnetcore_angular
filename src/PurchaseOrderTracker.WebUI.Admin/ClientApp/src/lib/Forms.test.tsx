import forms, { FormValue, FormValueMap } from './Forms';

describe('isFormValid', () => {
  const booleanValue: FormValue<boolean> = {
    value: true,
    touched: true,
  };
  const stringValue: FormValue<string> = {
    value: 'something',
    touched: true,
  };
  const stringArrayValue: FormValue<string[]> = {
    value: ['a', 'b'],
    touched: true,
  };

  test('returns true when form has no errors', () => {
    const formValues: FormValueMap = {
      booleanValue: Object.assign({}, booleanValue, { errors: undefined }),
      stringValue: Object.assign({}, stringValue, { errors: undefined }),
      stringArrayValue: Object.assign({}, stringArrayValue, { errors: undefined }),
    };

    expect(forms.isFormValid(formValues)).toBe(true);
  });

  test('returns false when form has errors', () => {
    const formValues: FormValueMap = {
      booleanValue: Object.assign({}, booleanValue, { errors: ['some error'] }),
    };

    expect(forms.isFormValid(formValues)).toBe(false);
  });
});
