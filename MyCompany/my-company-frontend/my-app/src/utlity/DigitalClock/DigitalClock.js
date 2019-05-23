import './DigitalClock.css'
import React from "react";

class DigitalClock extends React.Component{
    constructor() {
        super();
        this.state = {date: new Date()};
    }

    render(){
        return(<div>
            <h1 className="display-4 AlignRight">{this.state.date.toLocaleTimeString()}</h1>
        </div>);
    }

    componentDidMount(){
        this.timerID = setInterval(
            ()=>this.tick(),1000
        )
    }

    tick(){
        this.setState({
            date:new Date()
        });
    }


    componentWillUnmout(){
        clearInterval(this.timerID);
    }

}

export default DigitalClock;