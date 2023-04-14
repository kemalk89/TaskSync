import React, { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link, useLocation } from 'react-router-dom';
import { NavItemUser } from './nav-item-user/nav-item-user';
import { useAuth0 } from '@auth0/auth0-react';

export const NavMenu = () => {
  const [collapsed, setCollapsed] = useState(true);
  const location = useLocation();

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  };

  const { isAuthenticated } = useAuth0();

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">Times</NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow">
            {isAuthenticated && (
              <>
                <NavItem>
                  <NavLink active={location.pathname === "/"} tag={Link} to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={location.pathname.startsWith("/project")} tag={Link} to="/projects">Projects</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={location.pathname.startsWith("/ticket")} tag={Link} to="/tickets">Tickets</NavLink>
                </NavItem>
              </>
            )}
            <NavItemUser />
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
};
