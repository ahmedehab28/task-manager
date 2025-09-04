import signupStyles from './Signup.module.css'

import { FcGoogle } from 'react-icons/fc';
import { SiFacebook } from 'react-icons/si';

const Signup = () => {
    return (
        <main className={signupStyles.mainContainer}>
            <form className={signupStyles.signupForm} action="">
                <div className={signupStyles.content}>
                    <span className={signupStyles.title}>Sign Up</span>
                    <div className={signupStyles.names}>
                        <div>
                            <label htmlFor="firstName">First Name</label>
                            <input id="firstName" type="text" />
                        </div>
                        <div>
                            <label htmlFor="lastName">Last Name</label>
                            <input id="lastName" type="text" />
                        </div>
                    </div>
                    <div>
                        <label htmlFor="email">Email</label>
                        <input id="email" type="email" />
                    </div>

                    <div>
                        <label htmlFor="username">Username</label>
                        <input id="username" type="text" />
                    </div>

                    <div>
                        <label htmlFor="password">Password</label>
                        <input id="password" type="password" />
                    </div>
                    
                    <div>
                        <label htmlFor="confirmPassword">Confirm Password</label>
                        <input id="confirmPassword" type="password" />
                    </div>

                    <button onClick={(e) => e.preventDefault()} className={signupStyles.signupButton}>Create Account</button>

                    <div className={signupStyles.orContainer}>
                        <hr />
                        <span>OR</span>
                        <hr />
                    </div>                    
                    <div className={signupStyles.externalLogin}>
                        <button className={signupStyles.externalLoginButton}>
                            <FcGoogle 
                                size={24} 
                                className={` ${signupStyles.socialLogo} ${signupStyles.googleLogo}`}
                            />
                            Sign up with Google
                        </button>
                        <button className={signupStyles.externalLoginButton}>
                            <SiFacebook 
                                size={24} 
                                className={` ${signupStyles.socialLogo} ${signupStyles.facebookLogo}`}
                            />
                            Sign up with Facebook
                        </button>
                    </div>
                    <span>Already have an account? <a href="">login</a></span>
                </div>
            </form>
        </main>
    )
}

export default Signup