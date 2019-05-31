import React from 'react';
import './App.css';
import HomePage from "./containers/homePage/HomePage";
import Header from "./containers/header/Header";
import Footer from "./containers/footer/Footer";


export default class App extends React.Component{


    constructor() {
        super();
    }

    render(){
      return (
        <div className="App">
            <Header/>
            <HomePage/>
            <Footer/>
        </div>
      )
    }
}
