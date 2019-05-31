import React from "react";
import './HomePage.css';
import SlideShow from "./slideShow/SlideShow";
import ApplicationShow from "./applicationShow/ApplicationShow";
import SupportInfo from "./supportInfo/SupportInfo";

class HomePage extends React.Component{
    constructor() {
        super();
    }

    render(){
        return (<div className="homePage">
            <SlideShow/>
            <ApplicationShow/>
            <SupportInfo/>
        </div>);
    }
}

export default HomePage;