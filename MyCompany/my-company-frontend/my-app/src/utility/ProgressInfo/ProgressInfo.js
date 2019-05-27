import React from "react";
import "./ProgressInfo.css";

export default class ProgressInfo extends React.Component{


    constructor(props) {
        super(props);
        // this.state ={progress:props.progress}
    }

    render(){
        return(<h1 className={`display-4 AlignRight`}>{this.props.progress}</h1>)
    }

    componetDidMount(){

    }


    componentWillUnmount(){

    }
}