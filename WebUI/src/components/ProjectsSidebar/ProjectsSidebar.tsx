import sideStyles from './ProjectsSidebar.module.css'

import { NavLink } from 'react-router-dom'

import homeIcon from '../../assets/home.png'
import boardsIcon from '../../assets/boards-nav.png'
import arrowIcon from '../../assets/down-arrow.png'
import membersIcon from '../../assets/memebrs.png'
import settingsicon from '../../assets/settings.png'
import { useState } from 'react'

const sampleProjects = [
  { 
    id: '123',
    name: 'Trollo Project', 
    path: '/dashboard', 
  },
  {
    id: '321',
    name: 'Some Project',
    path: '/dashboard/some',
  },
  {
    id: '52',
    name: 'New Stuff Project',
    path: '/dashboard/new',
  }
]

const ProjectsSidebar = () => {
    const [openIDs, setOpenIDs] = useState<string[]>([])
    function openToggle (id : string) {
        setOpenIDs(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id])
    }

    const isOpen = (id : string) => openIDs.includes(id)
    const projectsElemnts = sampleProjects.map(project => {
        return (
            <li>
                <div onClick={() => openToggle(project.id)} className={sideStyles.projectHeader}>
                    <a href="/dashboard" aria-current="false">{project.name}</a>
                    <img className={isOpen(project.id) ? sideStyles.arrowUp : ""} src={arrowIcon} alt="arrow icon" />
                </div>
                <ul className={`${sideStyles.subList} ${isOpen(project.id) ? sideStyles.subListOpen : ""}`}>
                    <li>
                        <div className={sideStyles.subListHeader}>
                            <img src={boardsIcon} alt="boards icon" />
                            <a href={"/projects/"+project.id+"/boards"}>Boards</a>
                        </div>
                    </li>
                    <li>
                        <div className={sideStyles.subListHeader}>
                            <img src={membersIcon} alt="boards icon" />
                            <a href="/dashboard/overview">Members</a>
                        </div>
                    </li>
                    <li>
                        <div className={sideStyles.subListHeader}>
                            <img src={settingsicon} alt="settings icon" />
                            <a href="/dashboard/overview">Settings</a>
                        </div>
                    </li>
                </ul>
                
            </li>
        )
    })
    return (
            <aside className={sideStyles.sidebarLeft} aria-label="Navigation">
                <nav className={sideStyles.mainNav}>
                    <NavLink 
                        to="/workspace"
                        end
                        className={({isActive}) => 
                            (isActive ? `${sideStyles.activeMainNav}` : "")}
                    >
                        <img src={homeIcon} alt="home icon" />
                        Home
                    </NavLink>
                    <NavLink 
                        to="/workspace/boards"
                        className={({isActive}) => 
                            (isActive ? `${sideStyles.activeMainNav}` : "")}
                    >
                        <img src={boardsIcon} alt="boards icon" />
                        Boards
                    </NavLink>
                    <NavLink 
                        to="/workspace/projects"
                        className={({isActive}) => 
                            (isActive ? `${sideStyles.activeMainNav}` : "")}
                    >
                        Projects
                    </NavLink>
                </nav>
                <hr />
                <nav className={sideStyles.projectsNav}>
                    <span>Projects</span>
                    {projectsElemnts}
                </nav>
            </aside>
    )
}

export default ProjectsSidebar