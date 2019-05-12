import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display sign in form', () => {
    page.navigateTo();
    expect(page.getMainHeading()).toEqual('Sign In');
  });
});
