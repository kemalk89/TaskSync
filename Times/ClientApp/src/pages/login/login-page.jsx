import { useAuth0 } from "@auth0/auth0-react";

export const LoginPage = () => {
  const {
    loginWithRedirect,
    //isAuthenticated,
    //isLoading,
   // getAccessTokenSilently,
  } = useAuth0();

  const onLogin = () => loginWithRedirect();

  return (
    <div>
      You are not logged in. Please login to access application!
      <button onClick={onLogin}>Login</button>
    </div>
  );
};
