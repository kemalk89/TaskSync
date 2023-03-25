import React, { useEffect } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import {
  QueryClient,
  QueryClientProvider,
} from 'react-query'
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { useAuth0 } from "@auth0/auth0-react";
import './custom.css';
import { api } from './api/api';
import config from './config.json';

const queryClient = new QueryClient();

const App = () => {
  const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();


  useEffect(() => {
    if (!isLoading && !isAuthenticated) {
      navigate("/login");
    }
  }, [isAuthenticated, isLoading, navigate]);

  api.setAccessTokenLoader(getAccessTokenSilently, config.auth);

  return (
    <QueryClientProvider client={queryClient}>
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </Layout>
    </QueryClientProvider>
  );
}

export default App;
