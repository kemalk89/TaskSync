import React, { useEffect } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import {
  QueryCache,
  QueryClient,
  QueryClientProvider,
} from 'react-query'
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { useAuth0 } from "@auth0/auth0-react";
import { api } from './api/api';
import config from './config.json';
import { PageNotExists } from './pages/page-not-exists/page-not-exists';
import toast, { Toaster } from 'react-hot-toast';

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    // this is the global error handler for API calls
    onError: (err, query) => {
      if (query.observers.length > 0) {
        toast.error(() => (
          <p>{err.message}</p>
        ));
      }
    }
  })
});

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
        <Toaster />

        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
          
          <Route path="*" element={<PageNotExists />} />
        </Routes>
      </Layout>
    </QueryClientProvider>
  );
}

export default App;
