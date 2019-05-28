import React from 'react';
import './App.css';
import DigitalClock from './utility/DigitalClock/DigitalClock';
import WarningButton from './utility/ButtonTest/WarningButton';
import ProgressInfo from "./utility/ProgressInfo/ProgressInfo";


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
            <div className="myDiv">
                <div>1</div>
                <div>2</div>
                <div>3</div>
                <h1>你好</h1>
            </div>

            <ProgressInfo progress={this.state.myProgress}/>
            <DigitalClock />
            <WarningButton  progress={this.changeProgress.bind(this)} />
        </div>
      );
    }
}
