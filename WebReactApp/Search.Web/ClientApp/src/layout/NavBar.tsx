import { NavLink } from "react-router-dom";
import { Menu, Container } from "semantic-ui-react";
import logo from '../assets/react.svg';
import { observer } from "mobx-react-lite";

export default observer(function NavBar() {  
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item as={NavLink} to='/' header>
          <img
            src={logo}
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          Cognitive Search Examples
        </Menu.Item>  
        {<Menu.Item as={NavLink} to='/Hotels' name="Hotels" />  }    
      </Container>
    </Menu>
  );
})
