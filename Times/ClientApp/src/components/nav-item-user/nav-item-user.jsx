import { useAuth0 } from "@auth0/auth0-react";
import { NavLink, useNavigate } from "react-router-dom";
import {
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
  NavItem,
  UncontrolledDropdown,
} from "reactstrap";
import "./style.css";
import { UserName } from "../user-name/user-name";

export const NavItemUser = () => {
  const { isLoading, isAuthenticated, user, logout } = useAuth0();
  const navigate = useNavigate();

  if (isLoading) {
    return null;
  }

  if (!isAuthenticated) {
    return (
      <NavItem>
        <NavLink to="/components/">Login</NavLink>
      </NavItem>
    );
  }

  return (
    <UncontrolledDropdown nav inNavbar>
      <DropdownToggle nav caret>
        <UserName user={user} />
        {user.name}
      </DropdownToggle>
      <DropdownMenu end>
        <DropdownItem onClick={() => navigate("/profile")}>
          Profil anzeigen
        </DropdownItem>
        <DropdownItem>Einstellungen</DropdownItem>
        <DropdownItem divider />
        <DropdownItem
          onClick={() =>
            logout({ logoutParams: { returnTo: window.location.origin } })
          }
        >
          Logout
        </DropdownItem>
      </DropdownMenu>
    </UncontrolledDropdown>
  );
};
