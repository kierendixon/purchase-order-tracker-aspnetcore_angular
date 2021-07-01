import React, { useEffect, useState } from 'react';
import Site from './site/Site';
import { UserService } from './site/security/UserService';
import { faUserSlash } from '@fortawesome/free-solid-svg-icons';

const App: React.FC = () => {
  const [isAuthorised, setIsAuthorised] = useState<boolean | undefined>(undefined);

  // TODO: is this needed anymore? should use razor page to return a html file?
  // need to enable local debug / hot relaod though
  useEffect(() => {
    async function checkUserAccess() {
      var users = await fetch('/admin/user/currentUser'); // rename to /checksession or something
      if (!users.ok) {
        window.location.href = '/account';
      } else {
        setIsAuthorised(true);
      }
    }

    checkUserAccess();
  }, []);

  // if (authService.currentUser?.token == null) {
  //   authService.clearCurrentUser();
  //   window.location.href = '/account';
  //   return <div>Authentication Failed. Redirecting to Logon page...</div>;
  // }

  return isAuthorised ? <Site></Site> : <span>Loading...</span>;
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
