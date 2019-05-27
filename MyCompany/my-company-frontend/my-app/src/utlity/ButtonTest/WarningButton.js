import * as React from "react";

class WarningButton extends React.Component{

    constructor() {
        super();

        this.state= {isToggleOn:false};
        this.handleClick = this.handleClick.bind(this);
    }

    render(){


        return(<button type="button" className={`btn ${this.state.isToggleOn?'btn-success':'btn-danger'} float-right`} onClick={this.handleClick}>
            <h1 >{this.state.isToggleOn?"on":"off"}</h1>
        </button>);
    }

    handleClick(){
        this.props.progress(!this.state.isToggleOn?"true":"false");
        this.setState(state =>({
            isToggleOn: !state.isToggleOn
        }))

    }


    componentDidMount(){

    }

    componentWillUnmount(){

    }

}

export default WarningButton;