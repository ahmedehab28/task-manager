import homeStyles from './Home.module.css'

import { NavLink } from 'react-router-dom'
import MainLayout from '../layouts/MainLayout'

const Home = () => {
    return (

            <main className={homeStyles.contianer}>
                <NavLink to="/projects">
                    Go to your projects
                </NavLink>
            </main>
    )
}

export default Home