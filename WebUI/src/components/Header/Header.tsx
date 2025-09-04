import headerStyles from './Header.module.css'
import { NavLink } from 'react-router-dom'

import logo from '../../assets/logo.png'

const Header = () => {
    return (
        <header className={headerStyles.headerContainer}>
            <div className={headerStyles.logoContainer}>
                <img src={logo} alt="todo-logo" />
                <span>Trollo</span>
            </div>

            <div className={headerStyles.navContainer}>
                <NavLink to="/signup">
                    Signup
                </NavLink>
                <NavLink to="/login">
                    Login
                </NavLink>
            </div>
        </header>
    )
}

export default Header