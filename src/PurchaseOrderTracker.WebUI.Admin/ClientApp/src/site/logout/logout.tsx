import React from 'react';
import {AuthService} from '../security/AuthService';
import {LocalStorageService} from '../security/LocalStorageService';

const logout = (props: Props) => <div className={props.className} onClick={handleLogout}>Logout</div>

function handleLogout(){
    const storageService = new LocalStorageService(window.localStorage);
    const authService = new AuthService(storageService);

    authService.handleLogoutCommand();
}

export default logout;

interface Props{
    className: string;
}
