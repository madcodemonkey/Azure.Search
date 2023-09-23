import ReactDOM from 'react-dom/client';
import "semantic-ui-css/semantic.min.css";
import 'react-toastify/dist/ReactToastify.min.css'; 
import './index.css'
import { store, StoreContext } from "./stores/Store";
import { router } from "./routes/routes";
import { RouterProvider } from "react-router-dom";

ReactDOM.createRoot(document.getElementById('root')!).render(
  <StoreContext.Provider value={store}> 
      <RouterProvider router={router} />
  </StoreContext.Provider>
)
