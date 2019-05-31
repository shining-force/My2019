import React from "react";
import './NavBar.css'

const black_logo = "./resources/flower_logo_black.svg";
const colorful_logo = "./resources/flower_logo_colorful.svg";
const black_user_login = "./resources/user_login_black.svg";
const colorful_user_login = "./resources/user_login_colorful.svg";
const black_search = "./resources/search_black.svg";
const colorful_search = "./resources/search_colorful.svg";

const market = "Marketing";
const professionalProduct = "Professional";
const moreProduct="More Product";

const link_market = "https://www.taobao.com";
const link_professionalProduct = "https://www.taobao.com";

class NavBar extends React.Component{



    constructor(props) {
        super(props);
        this.state = {navBarHoverd:false,
            marketHoverd:false,
            professionalProduct:false
        };

    }

    render(){
        return(<div>
            <div className="navBar" onMouseEnter={()=> this.setState({navBarHoverd:true})} onMouseLeave={()=> this.setState({navBarHoverd:false})}>
            <div className="companyLogo">
                <ul>
                    <li>logo</li>
                </ul>
            </div>

            <div className="productCategory">
                <ul>
                    <li><a href={link_market} onMouseEnter={()=> this.setState({marketHoverd:true})} onMouseLeave={()=> this.setState({marketHoverd:false})}>{market}</a></li>
                    <li><a href={link_professionalProduct} onMouseEnter={()=> this.setState({professionalProduct:true})} onMouseLeave={()=> this.setState({professionalProduct:false})}>{professionalProduct}</a></li>
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
            <div className={this.state.navBarHoverd?'backgroundShow':'backgroundHide'}></div>
            </div>
            <div className="subMenus">
                <div className={`subMenu ${this.state.marketHoverd?'subMenuShow':'subMenuHide'}`}>This is "MarketPlace"</div>
                <div className={`subMenu ${this.state.professionalProduct?'subMenuShow':'subMenuHide'}`}>This is "ProfessionalProduct"</div>
            </div>

        </div>)
    }
}

export default NavBar;