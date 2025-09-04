import type { ReactNode } from "react"

import Header from '../../components/Header/Header'
import ProjectsSidebar from "../../components/ProjectsSidebar/ProjectsSidebar"
import mainLayOutStyles from './MainLayout.module.css'

import { Outlet } from "react-router-dom"


const MainLayout = () => {
    return (
        <main className={mainLayOutStyles.layOutContainer}>
            <Header />
            <div className={mainLayOutStyles.layout}>
                <ProjectsSidebar />
                <Outlet />
            </div>
        </main>
    )
}

export default MainLayout