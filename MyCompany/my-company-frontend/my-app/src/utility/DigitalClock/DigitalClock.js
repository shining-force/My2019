import './DigitalClock.css'
import React from "react";

class DigitalClock extends React.Component{
    constructor() {
        super();
        this.state = {date: new Date()};
    }

    render(){
        return(
            <h1 className="display-4 myblock">{this.state.date.toLocaleTimeString()}</h1>
        );
    }

    componentDidMount(){
        this.timerID = setInterval(
            ()=>this.tick(),1000
        )
    }

    tick(){
        //this.props.progress(new Date().toLocaleTimeString());
        this.setState({
            date:new Date()
        });
    }


    componentWillUnmout(){
        clearInterval(this.timerID);
    }

}

export default DigitalClock;