import { Auth0Provider } from '@auth0/auth0-react';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import config from './config.json';
import App from './App';

import 'bootstrap/dist/css/bootstrap.css';
import 'react-bootstrap-typeahead/css/Typeahead.css';
import 'react-bootstrap-typeahead/css/Typeahead.bs5.css';
import './custom.css';
import './styles.css';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(
  <Auth0Provider
    domain={config.auth.domain}
    clientId={config.auth.clientId}
    authorizationParams={{
      redirect_uri: window.location.origin,
      audience: config.auth.audience
    }}
  >
    <BrowserRouter basename={baseUrl}>
      <App />
    </BrowserRouter>
  </Auth0Provider>
);
