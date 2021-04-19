import React from 'react';
import Site from './site/site';
import { AuthService } from './site/security/AuthService';
import { LocalStorageService } from './site/security/LocalStorageService';

const App: React.FC = () => {
  const storageService = new LocalStorageService(window.localStorage);
  const authService = new AuthService(storageService);
  
  // if (authService.currentUser?.token == null) {
  //   authService.clearCurrentUser();
  //   window.location.href = '/account';
  //   return <div>Authentication Failed. Redirecting to Logon page...</div>;
  // }

  return <Site></Site>;
};

export default App;

/*

    if(is authenticated)
    {
        const LogggedInSite = UserProvider(Site);
        return <LoggedInSite></LoggedInSite>
    }

    redirectToLoginPage()
*/
