import loginStyles from './Login.module.css'

export default function Login () {
    return (
        <main>
            <form className={loginStyles.loginForm} action="">
                <label htmlFor="username">Username</label>
                <input id="username" type="text" />

                <label htmlFor="password">Password</label>
                <input id="password" type="text" />
            </form>
        </main>
    )
}