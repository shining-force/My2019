import React from "react";
import './NavBar.css'

const black_logo = "./resources/flower_logo_black.svg";
const colorful_logo = "./resources/flower_logo_colorful.svg";
const black_user_login = "./resources/user_login_black.svg";
const colorful_user_login = "./resources/user_login_colorful.svg";
const black_search = "./resources/search_black.svg";
const colorful_search = "./resources/search_colorful.svg";

const market = "市场";
const professionalProduct = "专业产品";
const moreProduct="更多产品";

const link_market = "https://www.taobao.com";
const link_professionalProduct = "https://www.taobao.com";

class NavBar extends React.Component{



    constructor(props) {
        super(props);
        this.state = {hoverd:false
        };

    }

    render(){
        return(<div className="navBar" onMouseEnter={()=> this.setState({hoverd:true})} onMouseLeave={()=> this.setState({hoverd:false})}>
            <div className="companyLogo">
                <ul>
                    <li>logo</li>
                </ul>
            </div>

            <div className="productCategory">
                <ul>
                    <li><a href={link_market}>{market}</a></li>
                    <li><a href={link_professionalProduct}>{professionalProduct}</a></li>
                </ul>
            </div>

            <div className="userLogin">
                <ul>
                    <li>1</li>
                    <li>2</li>
                    <li>3</li>
                    <li>4</li>
                </ul>
            </div>
            <div className={this.state.hoverd?'backgroundShow':'backgroundHide'}></div>
        </div>)
    }
}

export default NavBar;