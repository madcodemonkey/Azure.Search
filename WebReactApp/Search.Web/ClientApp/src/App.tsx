import './App.css'
import { ToastContainer } from 'react-toastify';
import ModalContainer from './common/modal/ModalContainer';
import { Outlet } from 'react-router-dom';

function App() {
    return (
        <>
            <ModalContainer />
            <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
            <div style={{ paddingTop: "3.5em" }}>
                <Outlet />
            </div>
        </>
    )
}

export default App
