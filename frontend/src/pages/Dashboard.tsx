import React from 'react';
import Navbar from '../components/Navbar';

const Dashboard: React.FC = () => {
  const username = localStorage.getItem('username'); // hoặc lấy từ API

  return (
    <main>
      <header className="header fixed">
        <div className="main-content">
          <div className="body">
            <img src="/src/assets/img/logo.svg" alt="Lessson." className="logo" />
            <nav className="nav">
              <ul>
                <li className="active">
                  <a href="#!">Home</a>
                </li>
                <li>
                  <a href="#!">Courses</a>
                </li>
                <li>
                  <a href="#!">Pricing</a>
                </li>
                <li>
                  <a href="#!">Reviews</a>
                </li>
              </ul>
            </nav>
            <div className="action">
              <a href="#!" className="btn btn-sign-up">Sign Up</a>
            </div>
          </div>
        </div>
      </header>
      {/*Hero*/}
      <div className="hero">
        <div className="main-content">
          <div className="body">
            {/* Hero left */}
            <div className="media-block">
              <img src="src/assets/img/hero-img.jpg" alt="Learn without limits and spread knowledge." className='img' />
              <div className="hero-summary">
                <div className="item">
                  <div className="icon">
                    <img src="src/assets/icons/list.svg" alt="" />
                  </div>
                  <div className="info">
                    <p className="label">20 Courses</p>
                    <p className="title">UI/UX Design</p>
                  </div>
                </div>
                <div className="item">
                  <div className="icon">
                    <img src="src/assets/icons/code.svg" alt="" />
                  </div>
                  <div className="info">
                    <p className="label">20 Courses</p>
                    <p className="title">Development</p>
                  </div>
                </div>
                <div className="item">
                  <div className="icon">
                    <img src="src/assets/icons/speaker.svg" alt="" />
                  </div>
                  <div className="info">
                    <p className="label">30 Courses</p>
                    <p className="title">Marketing</p>
                  </div>
                </div>
              </div>
            </div>
            {/* Hero right */}
            <div className="content-block">
              <h1 className="heading">
              Learn without limits and spread knowledge.
              </h1>
              <p className="desc">
              Build new skills for that “this is my year” feeling with courses, certificates, and degrees from world-class universities and companies.
              </p>
              <div className="cta-group">
                <a href="#!" className="btn hero-cta">See Courses</a>
                <button className="watch-video">
                  <div className="icon">
                    <img src="src/assets/icons/play.svg" alt="" />
                  </div>
                  <span>Watch Video</span>
                </button>
              </div>
              <p className="desc">Recent engagement</p>
              <p className="desc stats">
                <strong>50K</strong> Students
                <strong>70+</strong> Courses
              </p>
              
            </div>
          </div>
        </div>
      </div>
  </main>
  );
};

export default Dashboard;
