import { createBrowserRouter, Navigate, RouteObject } from 'react-router-dom';
import App from '../App';
import HomePage from '../features/home/HomePage';
import TestErrors from '../features/errors/TestError';
import NotFound from '../features/errors/NotFound';
import ServerError from '../features/errors/ServerError';
import HotelsPage from '../features/hotels/HotelsPage';


export const routes: RouteObject[] = [
  {
    path: '/',
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "hotels", element: <HotelsPage />},
      { path: "errors", element: <TestErrors /> },
      { path: "not-found", element: <NotFound /> },
      { path: "server-error", element: <ServerError /> },
      { path: "*", element: <Navigate replace to='/not-found' /> }
    ],
  }
];

export const router = createBrowserRouter(routes);