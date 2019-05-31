import React from "react";
import './Header.css';
import NavBar from "../../components/navBar/NavBar";

class Header extends React.Component{
    render(){
        return(
            <div className="header">
                <NavBar />
            </div>
        )
    }
}

export default Header;