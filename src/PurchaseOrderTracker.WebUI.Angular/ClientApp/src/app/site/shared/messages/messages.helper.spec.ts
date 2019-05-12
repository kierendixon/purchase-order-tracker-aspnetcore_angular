import { TestHelper } from '../../../../test/test-helper';
import { MessagesHelper } from './messages.helper';

describe('MessagesHelper', () => {
  describe('#ConvertErrorToFriendlyMessage', () => {
    it('converts Error type to message', () => {
      const error = TestHelper.buildError();
      const friendlyMessage = MessagesHelper.ConvertErrorToFriendlyMessage({ error });
      expect(friendlyMessage).toEqual(error.message);
    });

    it('converts HttpErrorResponse type to message', () => {
      const error = TestHelper.buildHttpErrorResponse();
      const friendlyMessage = MessagesHelper.ConvertErrorToFriendlyMessage(error);
      expect(friendlyMessage).toContain(error.status.toString());
    });
  });
});
