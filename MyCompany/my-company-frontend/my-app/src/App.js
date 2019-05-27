import React from 'react';
import './App.css';
import DigitalClock from './utlity/DigitalClock/DigitalClock';
import WarningButton from './utlity/ButtonTest/WarningButton';
import ProgressInfo from "./utlity/ProgressInfo/ProgressInfo";


export default class App extends React.Component{


    constructor() {
        super();
        this.state = {myProgress:"false"};
    }

    changeProgress(info){
        this.setState({myProgress:info});
    }

    render(){
      return (
        <div className="App">
            <ProgressInfo progress={this.state.myProgress}/>
            <DigitalClock />
            <WarningButton  progress={this.changeProgress.bind(this)} />
        </div>
      );
    }
}
