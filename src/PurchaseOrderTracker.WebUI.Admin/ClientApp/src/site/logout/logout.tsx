import React from 'react';

const logout = (props: Props) => <div className={props.className} onClick={handleLogout}>Logout</div>

function handleLogout(){
}

export default logout;

interface Props{
    className: string;
}
