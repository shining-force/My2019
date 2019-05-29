import React from "react";
import './NavBar.css'


class NavBar extends React.Component{

    constructor(props) {
        super(props);
    }

    render(){
        return(<div className="navBar">
            <ul>
                <li>1</li>
                <li>2</li>
                <li>3</li>
                <li>4</li>
            </ul>
        </div>)
    }
}

export default NavBar;